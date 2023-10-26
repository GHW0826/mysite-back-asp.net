
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

public class UserEntity : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; internal set; }
    public string Email { get; internal set; } = string.Empty;
    public string Password { get; internal set; } = string.Empty;
    public string Name { get; internal set; } = string.Empty;

    public static UserEntity Gen(string email, string password, string name)
    {
        return new UserEntity { Email = email, Password = password, Name = name};
    }

    public UserEntity ChangeEmail(string email)
    {
        this.Email = email;
        return this;
    }

    public UserEntity ChangePassword(string password)
    {
        this.Password = password;
        return this;
    }

    public UserEntity ChangeName(string name) 
    {  
        this.Name = name;
        return this;
    }
}
