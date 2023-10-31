
using Application.Auth.Model;
using Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Auth.Command;


public class UserNameUpdateCommand : IRequest<SaveModel>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UserNameUpdateCommandHandler : IRequestHandler<UserNameUpdateCommand, SaveModel>
{
    private readonly ILogger<UserNameUpdateCommand> _logger;

    public UserNameUpdateCommandHandler(ILogger<UserNameUpdateCommand> logger)
    {
        _logger = logger;
    }

    public async Task<SaveModel> Handle(UserNameUpdateCommand request, CancellationToken cancellationToken)
    {
        // UserEntity findUser = await _context.findById(request.Id) ?? throw new ArgumentException("Invalid User");
        // var result = await _context.save(findUser);

        return new SaveModel
        {
            Id = 0, //result.id,
            Email = "123", // result.email,
            Name = "123", //result.name
        };
    }
}