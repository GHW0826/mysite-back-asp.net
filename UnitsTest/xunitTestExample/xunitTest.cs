using System.Collections;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitsTest.xunitTestExample;


public class CalculatorTests
{
    public class Calculator
    {
        public int Add(int value1, int value2)
        {
            return value1 + value2;
        }
    }

    [Fact]
    public void CanAddxUnit()
    {
        var calculator = new Calculator();

        int value1 = 1;
        int value2 = 2;

        var result = calculator.Add(value1, value2);

        Assert.Equal(3, result);
    }

    [Fact(Timeout= 1000)]
    public async Task TimeoutTest()
    {
        await Task.Delay(2000);

    }
}