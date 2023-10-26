
namespace Application.Auth.Model;

public class SaveModel
{
    public ulong Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
