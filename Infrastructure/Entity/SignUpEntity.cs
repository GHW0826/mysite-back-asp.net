
namespace Infrastructure.Entity;

public class SignUpEntity : Entity
{
    public ulong id { get; set; }
    public string email { get; set; } = string.Empty;
    public string password { get; set;} = string.Empty;
}
