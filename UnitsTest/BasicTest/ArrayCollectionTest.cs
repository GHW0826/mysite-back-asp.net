using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace UnitsTest.BasicTest;

public class ArrayCollectionTest
{
    private ITestOutputHelper _logger;
    int rowSize = 20000;
    int colSize = 20000;
    int[,] array;
    List<List<int>> arrayList = new List<List<int>>();

    public ArrayCollectionTest(ITestOutputHelper logger)
    {
        _logger = logger;

        // raw array
        array = new int[rowSize, colSize];
        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                array[i, j] = i * colSize + j;
            }
        }

        for (int i = 0; i < rowSize; i++)
        {
            List<int> row = new List<int>();
            for (int j = 0; j < colSize; j++)
            {
                // 각 요소에 값을 추가하거나 초기화
                row.Add(i * colSize + j);
            }
            arrayList.Add(row);
        }
    }

    [Fact]
    public void arrayTimeTest()
    {


        long sum = 0;
        Stopwatch stopwatch = new Stopwatch(); //객체 선언
        stopwatch.Start(); // 시간측정 시작

        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                sum += array[i, j];
            }
        }
        _logger.WriteLine("sum: " + sum);
        stopwatch.Stop(); //시간측정 끝
        _logger.WriteLine("time : " + stopwatch.ElapsedMilliseconds + "ms");
    }

    [Fact]
    public void arrayCollectionTimeTest()
    {

        long sum = 0;
        Stopwatch stopwatch = new Stopwatch(); //객체 선언
        stopwatch.Start(); // 시간측정 시작

        for (int i = 0; i < rowSize; i++)
        {
            for (int j = 0; j < colSize; j++)
            {
                sum += arrayList[i][j];
            }
        }

        _logger.WriteLine("sum: " + sum);
        stopwatch.Stop(); //시간측정 끝
        _logger.WriteLine("time : " + stopwatch.ElapsedMilliseconds + "ms");
    }
}