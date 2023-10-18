using MediatR;
using mysite_back_asp.net.Model;
using mysite_back_asp.net.Repository;
using System;

namespace mysite_back_asp.net.queries
{
    public class TestQuery : IRequest<TestModel>
    {
        public string Pid { get; set; } = string.Empty;

        public int Param2 { get; set; }
    }

    public class TestQueryHandler : IRequestHandler<TestQuery, TestModel>
    {
        readonly MysqlContext _context;
        readonly ILogger<TestQuery> _logger;

        public TestQueryHandler(MysqlContext context, ILogger<TestQuery> logger)
        {
            _context = context;
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

            return new TestModel
            {
                Pid = "2",
                Point = 1000,
            };
        }
    }
}

