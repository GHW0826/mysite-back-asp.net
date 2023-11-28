using Moq;
using Xunit;

namespace UnitsTest.xunitTestExample;

public class xunitStubTest
{
    // stub은 실제 코드나 아직 준비되지 못한 코드의 행동으로 가장하는 메커니즘.
    // stub을 사용하면 시스템의 특정 부분이 준비되지 못했더라도 다른 부분을 테스트할 수 있다.
    // 보통 stub은 대상 코드에 손을 대지 않으면서 통합 된다는 장점이 있다.
    // stub은 포괄적인 코드 블록을 대체하는데 적합.
    // 파일 시스템, 서버 커넥션, 데이터벵이스 등 외부 시스템 전체를 대체하는데 주로 사용


    // Test Double : 실제 객체 대신 테스트 목적으로 사용되는 모든 종류의 가상 객체

    // 예를 들어, 은행에 송금하는 기능을 만든다고 가정.
    // 은행에 송금을 요청하는 인터페이스가 있을 것이고,
    // 매번 테스트할 때마다 송금 요청 인터페이스를 구현한 실제 객체를 사용하게 되면
    // 테스트가 실행될 때마다 내 돈이 어딘가로 송금될 수 있음
    // 이때 우리가 원하는건 실제로 은행에 송금을 요청하지는 않고 송금을 요청한 것처럼 행동한 뒤
    // 성공이나 실패 응답만 주는 객체

    // 이 객체를 통칭해서 Test Double이라고 부르고,
    // 테스트할 대상 자체는 테스트 코드에서 System Under Test(SUT)라고 함.
    // 아래 케이스는 TransferBank

    // 어떤 방식으로 이 객체를 구현하고 어떤 상황에서 사용하는지에 따라
    // Test Double의 종류가 나뉨
    // 1. Dummy
    //      객체 전달은 하지만 실제로 사용되지 않는 것.
    //      일반적으로 테스트할 대상을 구성하기 위해 값을 채우는 용도로만 사용.
    //      FROM 계좌의 잔액이 부족한 상황을 테스트하기 위해서는
    //      BankPort의 getBalance 구현과 sut.invoke의 인자로 주어진 amount 가 중요.
    //      sut을 구성하기 위해 전달해야 할 transferHistoryRepository와 emailPort는 사용되지 않기 때문에
    //      어떤 값이 입력되든 상관 없음
    //      val sut =   TransferBank (
    //                      transferHistoryRepository = mockk(), // Dummy 객체
    //                      bankPort = bankPortStub,
    //                      emailPort = mockk(),                // Dummy 객체
    //                  )
    // 2. Fake
    //      실제로 동작하는 구현을 가지고 있지만 일반적으로 프로덕션에서 적합하지 않은
    //      몇 가지 shourtcut을 사용하는 객체

    //      예를 들어, 프로덕션에서는 운영중인 MySQL 서버에 접속해서 데이터를 저장하고 조회하는 기능을 구현했다면,
    //      테스트 코드에서는 In-Memory Database를 사용해서 런타임에만 메모리에 데이터를
    //      저장하고 조회하는 기능을 구현할 수 있다.
    //      여기서 In-Memory Database가 Fake 객체에 해당.
    //      JDBC Driver를 사용한다면 MySQL 서버에 직접 접속할건지 H2와 같은 In-Memory Database를 사용할건지
    //      드라이버 수준에서 설정
    // 3. Stub
    //      테스트에 필요한 호출에 대해 미리 준비된 답을 제공하는 객체.
    //      일반적으로 테스트를 위해 작성된 기능 외에 다른 행동은 하지 않음.
    // 4. Spy
    //      어떤 기능이 어떻게 호출되었는지에 따라 일부 정보를 기록하는 종류
    // 5. Mock
    //      예상된 동작을 객체.
    //      Mock 사용시 어떤 호출을 기대하고 그 호출에 대한 결과가 무엇인지 명세를 만들어야 함.

    // stub은 answer (출력 검증), mock은 process (행동 검증) 느낌.
    // stub, mock 선택은 취향.

    // case.1) 특정 URL로 HTTP 커넥션을 맺어 컨텐츠를 가져오는 애플리케이션 stub
    // 원격 서버에서 HTML을 가져오는 예제를 재현 
    [Fact]
    public void HttpConnectionStubTest()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();
    }
}
