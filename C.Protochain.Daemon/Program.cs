using System;
using C.Protochain.Entities;
using C.Protochain.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace C.Protochain.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Protochain !");
            Console.WriteLine("Generating Genesis block");
            //BlockBusiness.BuildGenesisBlock();
            
            JObject parsed = JObject.Parse(JsonConvert.SerializeObject(DataStore.GetByIndex(0)));
            Console.WriteLine($"==============");
            foreach (var pair in parsed)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
            Console.ReadKey();
        }

    }
}
