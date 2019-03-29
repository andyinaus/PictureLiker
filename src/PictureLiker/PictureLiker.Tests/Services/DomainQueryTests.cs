using System;
using System.Linq.Expressions;
using Moq;
using PictureLiker.DAL;
using PictureLiker.DAL.Repositories;
using PictureLiker.Services;
using Xunit;

namespace PictureLiker.Tests.Services
{
    public class DomainQueryTests
    {
        [Fact]
        public void IsEmailAddressInUse_WhenEmailIsInUse_ShouldReturnTrue()
        {
            const string email = "a@a.com";

            var user = new User(new Mock<IDomainQuery>().Object);
            user.SetEmail(email).Wait();

            var userRepository = GetTestUserRepositoryWith(user);

            var domainQuery = new DomainQuery(userRepository);

            var isInUse = domainQuery.IsEmailInUse(email).Result;

            Assert.True(isInUse);
        }

        [Fact]
        public void IsEmailAddressInUse_WhenEmailIsNotInUse_ShouldReturnFalse()
        {
            var userRepository = GetTestUserRepositoryWith(null);

            var domainQuery = new DomainQuery(userRepository);

            const string email = "super@gmail.com";
            var isInUse = domainQuery.IsEmailInUse(email).Result;

            Assert.False(isInUse);
        }

        private static IRepository<User> GetTestUserRepositoryWith(User user)
        {
            var repo = new Mock<IRepository<User>>();

            repo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            return repo.Object;
        }
    }
}
