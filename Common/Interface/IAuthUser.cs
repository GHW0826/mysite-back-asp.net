namespace Water.Common.Interfaces;

public interface IAuthUser
{
    bool IsAuthenticated { get; }

    string Pid { get; }

    string Name { get; }

    string UserIp { get; }

    /// <summary>
    /// 리소스에 액세스할 수 있는 쉼표로 구분된 역할 목록을 가져옵니다.
    /// </summary>
    string Roles { get; }
}
