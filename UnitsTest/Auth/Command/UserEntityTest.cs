
using Application.Auth.Command;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitsTest.Auth.Command;

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
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.Email, "mysite@gmail.com");
        Assert.AreEqual(user.Password, "123");
        Assert.AreEqual(user.Name, "KIM");
    }

    [TestMethod]
    public void ChangeUserEmailEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeEmail("mysite2@gmail.com");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.Email, "mysite2@gmail.com");
    }

    [TestMethod]
    public void ChangeUserEmailNotEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeEmail("mysite2@gmail.com");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreNotEqual(user.Email, "mysite@gmail.com");
    }


    [TestMethod]
    public void ChangeUserPasswordEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangePassword("2345");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.Password, "2345");
    }

    [TestMethod]
    public void ChangeUserPasswordNotEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangePassword("2345");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreNotEqual(user.Password, "123");
    }


    [TestMethod]
    public void ChangeUserNameEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeName("LEE");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreEqual(user.Name, "LEE");
    }

    [TestMethod]
    public void ChangeUserNameNotEqualTest()
    {
        // Given: 테스트를 진행할 행위를 위한 사전 준비
        UserEntity user = UserEntity.Gen("mysite@gmail.com", "123", "KIM");

        // when: 테스트를 진행할 행위
        user.ChangeName("LEE");

        // then: 테스트를 진행한 행위에 대한 결과 검증
        Assert.AreNotEqual(user.Email, "KIM");
    }
}