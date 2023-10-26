using Application.Auth.Model;
using Application.Helper;
using Application.Interface;
using Domain.Entity;
using MediatR;
using Water.Common.Exceptions;

namespace Application.Auth.Command;


/// <summary>
/// Sign Up 계정 추가
/// </summary>
public class SignUpCommand : IRequest<SaveModel>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// SignUpCommand 를 처리합니다.
/// </summary>
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SaveModel>
{
    private readonly IAuthDbContext _context;
    private readonly IDistributedLockContext _distrulableLockContext;

    public SignUpCommandHandler(IAuthDbContext context, IDistributedLockContext distrulableLockContext)
    {
        _context = context;
        _distrulableLockContext = distrulableLockContext;
    }

    public async Task<SaveModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        bool locked = await _distrulableLockContext.AqurieLock(request.Email, "1");
        if (!locked)
            throw new AuthorizationException("exist emailttt");

        var checkEmail = await _context.findByEmail(request.Email);
        if (checkEmail != null)
            throw new AuthorizationException("exist email23");

        UserEntity newUser = UserEntity.Gen(request.Email, HashHelper.EncryptPassword(request.Password), request.Name);
        var result = await _context.save(newUser);

        await _distrulableLockContext.ReleaseLock(request.Email, "1");

        return new SaveModel
        {
            Id = result.Id,
            Email = result.Email,
            Name = result.Name
        };
    }
}
