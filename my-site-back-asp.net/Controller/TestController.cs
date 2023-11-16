using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SnowFall.AspNetCore;
using Infra.Repository;

namespace mysite_back_asp.net.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController : BaseApiController
{
    private readonly ILogger<TestController> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly TestContext _context;

    public TestController(IDistributedCache distributedCache, TestContext context, ILogger<TestController> logger)
    {
        _distributedCache = distributedCache;
        _context = context;
        _logger = logger;
    }


    [HttpGet("test")]
    public IActionResult test()
    {
        return Unauthorized();
    }

    [HttpGet("test2")]
    public IActionResult test2()
    {
        return Unauthorized();
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


    [HttpGet("{id}")]
    public async Task<ActionResult<TestEntity>> GetTodoItem(long id)
    {
        var todoItem = await _context.TestEntitys.FindAsync(id);
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
