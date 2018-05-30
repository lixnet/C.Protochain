using System.Collections.Generic;
using C.Protochain.Business;
using C.Protochain.Entities;
using C.Protochain.Node.Models;
using C.Protochain.Store;
using Microsoft.AspNetCore.Mvc;

namespace C.Protochain.Node.Controllers
{
    [Route("api/[controller]")]
    public class BlocksController : Controller
    {
        public BlocksController()
        {
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<Block> Get()
        {
            return DataStore.GetBlockchain();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Block Get(int id)
        {
            return DataStore.GetByIndex(id);
        }

        // POST api/values
        [HttpPost]
        public Block Post([FromBody]BlockData blockData)
        {
            return BlockBusiness.GenerateNextBlock(blockData.Data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
