using System;
using Xunit;
using PictureLiker.DAL;

namespace PictureLiker.Tests.DAL
{
    public class UserTests
    {
        [Fact]
        public void SetEmailWithNullEmailShouldThrowArgumentNullException()
        {
            var user = new User();

            Action action = () => user.SetEmail(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetEmailWithWhiteSpaceEmailShouldThrowArgumentNullException()
        {
            var user = new User();

            Action action = () => user.SetEmail("    ");

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetEmailWithInvalidEmailShouldThrowArgumentException()
        {
            var user = new User();
            const string invalidEmail = "hahaahhahdha";

            Action action = () => user.SetEmail(invalidEmail);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void SetEmailWithValidEmailShouldSetCorrectly()
        {
            var user = new User();
            const string validEmail = "a@gmail.com";

            user.SetEmail(validEmail);

            Assert.Equal(validEmail, user.Email);
        }

        [Fact]
        public void SetNameWithNullNameShouldThrowArgumentNullException()
        {
            var user = new User();

            Action action = () => user.SetName(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetNameWithWhiteSpaceNameShouldThrowArgumentNullException()
        {
            var user = new User();

            Action action = () => user.SetName("    ");

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetNameWithValidNameShouldSetCorrectly()
        {
            var user = new User();
            const string validName = "Andy";

            user.SetName(validName);

            Assert.Equal(validName, user.Name);
        }

        [Fact]
        public void SetRoleWithNullRoleShouldThrowArgumentNullException()
        {
            var user = new User();

            Action action = () => user.SetRole(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetRoleWithWhiteSpaceRoleShouldThrowArgumentNullException()
        {
            var user = new User();

            Action action = () => user.SetRole("    ");

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetRoleWithInvalidRoleShouldThrowArgumentException()
        {
            var user = new User();

            Action action = () => user.SetRole("SuperUser");

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void SetRoleWithAdminRoleShouldSetCorrectly()
        {
            var user = new User();

            user.SetRole(Authentication.RoleTypes.Administrator);

            Assert.Equal(Authentication.RoleTypes.Administrator, user.Role);
        }

        [Fact]
        public void SetRoleWithGeneralUserRoleShouldSetCorrectly()
        {
            var user = new User();

            user.SetRole(Authentication.RoleTypes.GeneralUser);

            Assert.Equal(Authentication.RoleTypes.GeneralUser, user.Role);
        }
    }
}
