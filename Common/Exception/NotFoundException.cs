namespace Water.Common.Exceptions;

/// <summary>
/// 찾는 대상이 존재하지 않을때 throw되는 예외입니다.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// 사용되지 않습니다. 
    /// </summary>
    /// <param name="code">예외에 대한 이유를 정의하는 4자리의 코드입니다.</param>
    /// <param name="target">찾을 수 없는 대상입니다.</param>
    [Obsolete("Code를 지정하는 생성자는 더 이상 사용할 수 없습니다.")]
    public NotFoundException(string code, string target)
        : base("대상을 찾을 수 없습니다.")
    {
        Target = target;
    }

    /// <summary>
    /// 사용되지 않습니다. 
    /// </summary>
    /// <param name="code">예외에 대한 이유를 정의하는 4자리의 코드입니다.</param>
    /// <param name="target">찾을 수 없는 대상입니다.</param>
    /// <param name="message">예외에 대한 이유를 설명하는 오류 메시지입니다.</param>
    [Obsolete("Code를 지정하는 생성자는 더 이상 사용할 수 없습니다.")]
    public NotFoundException(string code, string target, string message)
        : base(message)
    {
        Target = target;
    }

    /// <summary>
    /// 사용되지 않습니다. 
    /// </summary>
    /// <param name="code">예외에 대한 이유를 정의하는 4자리의 코드입니다.</param>
    /// <param name="target">찾을 수 없는 대상입니다.</param>
    /// <param name="innerException">현재 예외의 원인인 예외입니다.</param>
    [Obsolete("Code를 지정하는 생성자는 더 이상 사용할 수 없습니다.")]
    public NotFoundException(string code, string target, Exception innerException)
        : base("대상을 찾을 수 없습니다.", innerException)
    {
        Target = target;
    }

    /// <summary>
    /// 사용되지 않습니다. 
    /// </summary>
    /// <param name="code">예외에 대한 이유를 정의하는 4자리의 코드입니다.</param>
    /// <param name="target">찾을 수 없는 대상입니다.</param>
    /// <param name="message">예외에 대한 이유를 설명하는 오류 메시지입니다.</param>
    /// <param name="innerException">현재 예외의 원인인 예외입니다.</param>
    [Obsolete("Code를 지정하는 생성자는 더 이상 사용할 수 없습니다.")]
    public NotFoundException(string code, string target, string message, Exception innerException)
        : base(message, innerException)
    {
        Target = target;
    }

    //public NotFoundException(string target, string message, Exception innerException)
    //    : base(message, innerException)
    //{
    //    Target = target;
    //}

    /// <summary>
    /// NotFoundException 를 초기화 합니다.
    /// </summary>
    /// <param name="target">찾을 수 없는 대상입니다.</param>
    /// <param name="innerException">현재 예외의 원인인 예외입니다.</param>
    public NotFoundException(string target, Exception? innerException)
        : base("대상을 찾을 수 없습니다.", innerException)
    {
        Target = target;
    }

    //public NotFoundException(string target, string message)
    //    : this(target : target, message : message, innerException : null)
    //{ }

    /// <summary>
    /// NotFoundException 를 초기화 합니다.
    /// </summary>
    /// <param name="target">찾을 수 없는 대상입니다.</param>
    public NotFoundException(string target)
        : this(target, innerException: null)
    { }


    /// <summary>
    /// 예외 코드를 가져옵니다.
    /// </summary>
    public string Code => "0103";

    /// <summary>
    /// 찾을 수 없는 대상을 가져옵니다.
    /// </summary>
    public string Target { get; }

    public override string ToString()
    {
        return $"Code:{Code}, Target:{Target}\r\n {base.ToString()}";
    }

    public NotFoundException WithData(object key, object value)
    {
        this.Data[key] = value;
        return this;
    }
}