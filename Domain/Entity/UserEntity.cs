
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

public class UserEntity : Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long id { get; internal set; }
    public string email { get; internal set; } = string.Empty;
    public string password { get; internal set; } = string.Empty;
    public string name { get; internal set; } = string.Empty;

    public static UserEntity Gen(long id, string email, string password, string name)
    {
        return new UserEntity {
            id = id,
            email = email,
            password = password,
            name = name
        };
    }

    public UserEntity ChangeEmail(string email)
    {
        this.email = email;
        return this;
    }

    public UserEntity ChangePassword(string password)
    {
        this.password = password;
        return this;
    }

    public UserEntity ChangeName(string name) 
    {  
        this.name = name;
        return this;
    }
}
