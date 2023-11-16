

using Application.Test;
using Application.Test.Model;
using Microsoft.Extensions.Logging;

namespace Infra.adapter;

public class TestAdapter : ITestAdapter
{
    readonly ILogger<TestAdapter> _logger;

    public TestAdapter(ILogger<TestAdapter> logger)
    {
        _logger = logger;
    }

    public async Task<TestModel> GetTest()
    {
        return new TestModel
        {
            Pid = "1",
            Point = 100
        };
    }
}
