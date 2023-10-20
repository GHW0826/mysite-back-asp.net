
using Application.Test.Model;

namespace Application.Test;

public interface ITestAdapter
{
    Task<TestModel> GetTest();
}
