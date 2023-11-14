using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitsTest.xunitTestExample;

public class xunitExceptionTest
{
    internal static async Task ThrowExceptionAsync(string queryString)
    {
        throw new InvalidOperationException();

        //do something
        await Task.Run(() =>
        {

        });
    }

    [Fact]
    public async Task TestMethod_ShouldThrowExceptionWithSpecificMessage()
    {
        //Arrange
        var queryString = "SELECT * FROM c";

        //Act
        var exception = await Record.ExceptionAsync(() => xunitExceptionTest.ThrowExceptionAsync(queryString));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }
}
