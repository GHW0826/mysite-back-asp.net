using Application.User.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.User.Command;


public class UserUpdateCommand : IRequest<SignUpResponseModel>
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UserNameUpdateCommandHandler : IRequestHandler<UserUpdateCommand, SignUpResponseModel>
{
    private readonly ILogger<UserUpdateCommand> _logger;

    public UserNameUpdateCommandHandler(ILogger<UserUpdateCommand> logger)
    {
        _logger = logger;
    }

    public async Task<SignUpResponseModel> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        // UserEntity findUser = await _context.findById(request.Id) ?? throw new ArgumentException("Invalid User");
        // var result = await _context.save(findUser);

        return new SignUpResponseModel
        {
            id = 0, //result.id,
            email = "123", // result.email,
            name = "123", //result.name
        };
    }
}