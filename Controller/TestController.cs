using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using mysite_back_asp.net.Entity;
using mysite_back_asp.net.Repository;
using Newtonsoft.Json;
using System.Text;

namespace mysite_back_asp.net.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly MysqlContext _context;

        public TestController(IDistributedCache distributedCache, MysqlContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        [NonAction]
        public new OkObjectResult Ok()
        {
            return Ok(null);
        }

        [HttpGet("string")]
        public ActionResult<IEnumerable<string>> RegionSetSearch()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "customerList";
            await _distributedCache.SetStringAsync(cacheKey, "Hello");
            //  string serializedCustomerList;
            string redisCustomerList = await _distributedCache.GetStringAsync(cacheKey) ?? "";
            /*
            if (redisCustomerList != null)
            {
                serializedCustomerList = Encoding.UTF8.GetString(redisCustomerList);
                customerList = JsonConvert.DeserializeObject<List<Customer>>(serializedCustomerList);
            }
            else
            {
                customerList = await context.Customers.ToListAsync();
                serializedCustomerList = JsonConvert.SerializeObject(customerList);
                redisCustomerList = Encoding.UTF8.GetBytes(serializedCustomerList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCustomerList, options);
            }
            */
            return Ok(redisCustomerList);
        }


        /// <summary>
        /// Deletes a specific TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TestEntity>> GetTodoItem(long id)
        {
            var todoItem = await _context.TestEntitys.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<TestEntity>> PostTodoItem(TestEntity Item)
        {
            _context.TestEntitys.Add(Item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id = Item.Id }, Item);
        }
    }
}
