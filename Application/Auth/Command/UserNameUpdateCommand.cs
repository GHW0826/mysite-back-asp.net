
using Application.Auth.Model;
using Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Auth.Command;


public class UserNameUpdateCommand : IRequest<SaveModel>
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UserNameUpdateCommandHandler : IRequestHandler<UserNameUpdateCommand, SaveModel>
{
    private readonly IAuthDbContext _context;
    private readonly ILogger<UserNameUpdateCommand> _logger;

    public UserNameUpdateCommandHandler(IAuthDbContext context, ILogger<UserNameUpdateCommand> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SaveModel> Handle(UserNameUpdateCommand request, CancellationToken cancellationToken)
    {
        UserEntity findUser = await _context.findById(request.Id) ?? throw new ArgumentException("Invalid User");
        var result = await _context.save(findUser);

        return new SaveModel
        {
            Id = result.Id,
            Email = result.Email,
            Name = result.Name
        };
    }
}