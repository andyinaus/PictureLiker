using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PictureLiker.DAL.Entities;
using PictureLiker.Services;

namespace PictureLiker.Tests.DAL
{
    public class UserTests
    {
        [Fact]
        public async Task SetEmailWithNullEmailShouldThrowArgumentNullException()
        {
            var user = GetTestUser();

            Func<Task> target = () => user.SetEmail(null);

            await Assert.ThrowsAsync<ArgumentNullException>(target);
        }

        [Fact]
        public async Task SetEmailWithWhiteSpaceEmailShouldThrowArgumentNullException()
        {
            var user = GetTestUser();

            Func<Task> target = () => user.SetEmail("    ");

            await Assert.ThrowsAsync<ArgumentNullException>(target);
        }

        [Fact]
        public async Task SetEmailWithInvalidEmailShouldThrowArgumentException()
        {
            var user = GetTestUser();
            const string invalidEmail = "hahaahhahdha";

            Func<Task> target = () => user.SetEmail(invalidEmail);

            await Assert.ThrowsAsync<ArgumentException>(target);
        }

        [Fact]
        public async Task SetEmailWithValidEmailShouldSetCorrectly()
        {
            var user = GetTestUser();
            const string validEmail = "a@gmail.com";

            await user.SetEmail(validEmail);

            Assert.Equal(validEmail, user.Email);
        }

        [Fact]
        public void SetNameWithNullNameShouldThrowArgumentNullException()
        {
            var user = GetTestUser();

            Action action = () => user.SetName(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetNameWithWhiteSpaceNameShouldThrowArgumentNullException()
        {
            var user = GetTestUser();

            Action action = () => user.SetName("    ");

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetNameWithValidNameShouldSetCorrectly()
        {
            var user = GetTestUser();
            const string validName = "Andy";

            user.SetName(validName);

            Assert.Equal(validName, user.Name);
        }

        [Fact]
        public void SetRoleWithNullRoleShouldThrowArgumentNullException()
        {
            var user = GetTestUser();

            Action action = () => user.SetRole(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetRoleWithWhiteSpaceRoleShouldThrowArgumentNullException()
        {
            var user = GetTestUser();

            Action action = () => user.SetRole("    ");

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetRoleWithInvalidRoleShouldThrowArgumentException()
        {
            var user = GetTestUser();

            Action action = () => user.SetRole("SuperUser");

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void SetRoleWithAdminRoleShouldSetCorrectly()
        {
            var user = GetTestUser();

            user.SetRole(Authentication.RoleTypes.Administrator);

            Assert.Equal(Authentication.RoleTypes.Administrator, user.Role);
        }

        [Fact]
        public void SetRoleWithGeneralUserRoleShouldSetCorrectly()
        {
            var user = GetTestUser();

            user.SetRole(Authentication.RoleTypes.GeneralUser);

            Assert.Equal(Authentication.RoleTypes.GeneralUser, user.Role);
        }

        private static User GetTestUser(IDomainQuery domainQuery = null)
        {
            return new User(domainQuery ?? new Mock<IDomainQuery>().Object);
        }
    }
}
