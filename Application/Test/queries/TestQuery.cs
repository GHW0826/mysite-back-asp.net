using Application.Test.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Test.queries;

public class TestQuery : IRequest<TestModel>
{
    public string Pid { get; set; } = string.Empty;

    public int Param2 { get; set; }
}

public class TestQueryHandler : IRequestHandler<TestQuery, TestModel>
{
  //  readonly ITestDbContext _context;
    readonly ITestAdapter _adapter;
    readonly ILogger<TestQuery> _logger;

    public TestQueryHandler(ITestAdapter adapter, ILogger<TestQuery> logger)
    {
        _adapter = adapter;
    //    _context = context;
        _logger = logger;
    }

    public async Task<TestModel> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        /*
        GameInfo userGameInfo = await _context.GameInfo
            .FirstOrDefaultAsync(r => r.Pid == request.Pid, cancellationToken: cancellationToken);

        if (userGameInfo == null)
            throw new NotFoundException(request.Pid);
        */

        return await _adapter.GetTest();
    }
}

