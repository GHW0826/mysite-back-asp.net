namespace Application.Interface.Sharding;

public interface IShardContextPoolInterface
{
    public IShardManageContext? GetContext(string name);
}
