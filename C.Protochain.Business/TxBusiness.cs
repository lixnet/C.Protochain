using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using C.Protochain.Entities;
using C.Protochain.Utils;

namespace C.Protochain.Business
{
    public static class TxBusiness
    {
        public static long CoinbaseAmount = 50;

        public static string GetTransactionId(Transaction transaction)
        {
            string txInContent = string.Join(string.Empty, transaction.TransactionsIn.Select(_ => _.TransactionOutId + _.TransactionOutIndex));
            string txOutContent = string.Join(String.Empty, transaction.TransactionsOut.Select(_ => _.Address + _.Amount));
            string hash = (txInContent + txOutContent).ComputeSha256Hash();

            return hash;
        }

        public static string SignTransaction(Transaction transaction, 
                                             long transactionInIndex, 
                                             string privateKey, 
                                             UnspentTransactionOut[] unspentTransactionsOut)
        {
            TransactionIn transactionIn = transaction.TransactionsIn[transactionInIndex];
            string dataToSign = transaction.Id;
      
            UnspentTransactionOut refUnspentTransaction = FindUnspentTxOut(transactionIn.TransactionOutId, transactionIn.TransactionOutIndex, unspentTransactionsOut).FirstOrDefault();
            if (refUnspentTransaction == null)
                throw new Exception("Could not find the referenced transaction out.");

            var referencedAddress = refUnspentTransaction.Address;

            // Verify that the public key generated with the private key == referencedAddress, if false throw;

            string signature64;
            byte[] privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);
            var publicKey = CngKey.Import(privateKeyBytes, CngKeyBlobFormat.EccFullPrivateBlob);

            using (ECDsaCng dsa = new ECDsaCng(publicKey))
            {
                byte[] dataAsBytes = Encoding.UTF8.GetBytes(dataToSign);

                //CngProperty prop = new CngProperty("privateKey", privateKeyBytes, CngPropertyOptions.None);
                //dsa.Key.SetProperty(prop);
                byte[] signature = dsa.SignData(dataAsBytes);
                signature64 = Convert.ToBase64String(signature);
            }

            return signature64;
        }
        
        public static UnspentTransactionOut[] UpdateUnspentTransactionOuts(Transaction[] newTransactions, UnspentTransactionOut[] unspentTransactionsOut)
        {
            IEnumerable<UnspentTransactionOut> updatedUnspentTransactionsOut = newTransactions.Select((t) => t.TransactionsOut.Select((txOut, i) => new UnspentTransactionOut(t.Id, i, txOut.Address, txOut.Amount)))
            .Aggregate((a, b) => a.Concat(b));

            UnspentTransactionOut[] consumedTransactionsOut = newTransactions.Select((t) => t.TransactionsIn)
                .Aggregate((a, b) => a.Concat(b).ToArray())
                .Select((txIn) => new UnspentTransactionOut(txIn.TransactionOutId, txIn.TransactionOutIndex, string.Empty, 0)).ToArray();

            var resultingUnspentTxOuts = updatedUnspentTransactionsOut.Concat(unspentTransactionsOut.Where(u => !FindUnspentTxOut(u.TransactionOutId, u.TransactionOutIndex, consumedTransactionsOut).Contains(u)).ToList());

            return resultingUnspentTxOuts.ToArray();
        }

        public static bool ValidateTransaction(Transaction transaction, UnspentTransactionOut[] unspentTransactionOuts)
        {
            if (!ValidateTransactionStructure(transaction))
                return false;

            if (!ValidateTransactionId(transaction))
                return false;

            var transactionInsValidity = transaction.TransactionsIn.Select(txIn => ValidateTransactionIns(txIn, transaction, unspentTransactionOuts));
            if (transactionInsValidity.Any(t => t == false))
                return false;

            return ValidateTransactionBalancing(transaction, unspentTransactionOuts);
        }

        public static bool ValidateCoinbaseTransaction(Transaction transaction, long blockIndex)
        {
            if (GetTransactionId(transaction) != transaction.Id)
                return false;

            if (transaction.TransactionsIn.Length != 1)
                return false;

            if (transaction.TransactionsOut.Length != 1)
                return false;

            if (transaction.TransactionsIn[0].TransactionOutIndex != blockIndex)
                return false;

            return transaction.TransactionsOut[0].Amount == CoinbaseAmount;
        }

        private static bool ValidateTransactionIns(TransactionIn txIn, Transaction transaction, UnspentTransactionOut[] unspentTxsOut)
        {
            var referencedUTxOut = unspentTxsOut.FirstOrDefault((uTxO) => uTxO.TransactionOutId == txIn.TransactionOutId && uTxO.TransactionOutIndex == txIn.TransactionOutIndex);
            if (referencedUTxOut == null)
                return false;

            var address = referencedUTxOut.Address;
            byte[] signature64 = Convert.FromBase64String(txIn.Signature);
            var publicKey = CngKey.Import(signature64, CngKeyBlobFormat.EccFullPublicBlob);
            using (ECDsaCng dsa = new ECDsaCng(publicKey))
            {
                byte[] transactionIdBytes = Encoding.UTF8.GetBytes(transaction.Id);
                
                if (!dsa.VerifyData(transactionIdBytes, signature64))
                    return false;
            }

            return true;
        }

        private static long GetTxInAmmount(TransactionIn transactionIn, UnspentTransactionOut[] unspentTransactionOuts)
        {
            var unspentAmount = FindUnspentTxOut(transactionIn.TransactionOutId, transactionIn.TransactionOutIndex, unspentTransactionOuts).FirstOrDefault();
            if (unspentAmount != null)
            {
                return unspentAmount.Amount;
            }

            return 0;
        }

        private static bool ValidateTransactionBalancing(Transaction transaction, UnspentTransactionOut[] unspentTransactionOuts)
        {
            var totalTxInValues = transaction.TransactionsIn.Select(txIn => GetTxInAmmount(txIn, unspentTransactionOuts)).Aggregate((a, b) => a + b);
            var totalTxOutValues = transaction.TransactionsOut.Select(txOut => txOut.Amount).Aggregate((a, b) => (a + b));

            return totalTxInValues == totalTxOutValues;
        }

        private static bool ValidateTransactionId(Transaction transaction)
        {
            return GetTransactionId(transaction) == transaction.Id;
        }

        private static bool ValidateTransactionStructure(Transaction transaction)
        {
            // Improve with Fluent Validation !
            if (string.IsNullOrWhiteSpace(transaction.Id))
                return false;

            if (!transaction.TransactionsIn.Any())
                return false;

            if (!transaction.TransactionsOut.Any())
                return false;

            return true;
        }
        
        private static UnspentTransactionOut[] FindUnspentTxOut(string transactionId, long index, UnspentTransactionOut[] unspentTxOut)
        {
            return unspentTxOut.Where(tx =>
                tx.TransactionOutId == transactionId &&
                tx.TransactionOutIndex == index).ToArray();
        }
    }
}
