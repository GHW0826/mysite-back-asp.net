
using Domain.Entity;

namespace UnitsTest.Auth.EntityTest;

[TestClass]
public class UserEntityTest
{
    // Given: 테스트를 진행할 행위를 위한 사전 준비
    // when: 테스트를 진행할 행위
    // then: 테스트를 진행한 행위에 대한 결과 검증

    //함수명 : 시나리오_예상결과

    // [Fact]
    [TestMethod]
    public void SaveUserTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.email, "mysite@gmail.com");
        Assert.AreEqual(user.password, "123");
        Assert.AreEqual(user.name, "KIM");
    }

    [TestMethod]
    public void ChangeUserEmailEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeEmail("mysite2@gmail.com");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.email, "mysite2@gmail.com");
    }

    [TestMethod]
    public void ChangeUserEmailNotEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeEmail("mysite2@gmail.com");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreNotEqual(user.email, "mysite@gmail.com");
    }


    [TestMethod]
    public void ChangeUserPasswordEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangePassword("2345");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.password, "2345");
    }

    [TestMethod]
    public void ChangeUserPasswordNotEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangePassword("2345");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreNotEqual(user.password, "123");
    }


    [TestMethod]
    public void ChangeUserNameEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeName("LEE");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.name, "LEE");
    }

    [TestMethod]
    public void ChangeUserNameNotEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen(1, "mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeName("LEE");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreNotEqual(user.email, "KIM");
    }
}