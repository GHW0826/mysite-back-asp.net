using System.Collections;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitsTest.xunitTestExample;

// parameterised test example (exist junit)



public class xunitParameterisedTest
{
    public class Calculator
    {
        public int Add(int value1, int value2)
        {
            return value1 + value2;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Using the [Theory] attribute to create parameterised tests with [InlineData]

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-4, -6, -10)]
    [InlineData(-2, 2, 0)]
    [InlineData(int.MinValue, -1, int.MaxValue)]
    public void CanAddParameterisedTest1(int value1, int value2, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Add(value1, value2);

        /*
                   1,  2,          3
                  -4, -6,        -10
                  -2,  2,          0
         -2147483648, -1, 2147483647
        */
        Assert.Equal(expected, result);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Using a dedicated data class with [ClassData]

    public class CalculatorTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { -4, -6, -10 };
            yield return new object[] { -2, 2, 0 };
            yield return new object[] { int.MinValue, -1, int.MaxValue };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(CalculatorTestData))]
    public void CanAddParameterisedTest2(int value1, int value2, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Add(value1, value2);

        /*
                   1,  2,          3
                  -4, -6,        -10
                  -2,  2,          0
         -2147483648, -1, 2147483647
        */
        Assert.Equal(expected, result);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Loading data from a property on the test class

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };

    [Theory]
    [MemberData(nameof(Data))]
    public void CanAddParameterisedTest3(int value1, int value2, int expected)
    {
        var calculator = new Calculator();

        var result = calculator.Add(value1, value2);

        Assert.Equal(expected, result);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Loading data from a method on the test class

    public static IEnumerable<object[]> GetData(int numTests)
    {
        var allData = new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };

        return allData.Take(numTests);
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 3)]
    public void CanAddParameterisedTest4(int value1, int value2, int expected)
    {
        var calculator = new Calculator();
        var result = calculator.Add(value1, value2);

        Assert.Equal(expected, result);
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Loading data from a property or method on a different class

    [Theory]
    [MemberData(nameof(CalculatorData.Data), MemberType = typeof(CalculatorData))]
    public void CanAddParameterisedTest5(int value1, int value2, int expected)
    {
        var calculator = new Calculator();
        var result = calculator.Add(value1, value2);

        Assert.Equal(expected, result);
    }
}

public class CalculatorData
{
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };
}