using System;
using PictureLiker.DAL;
using PictureLiker.Services;

namespace PictureLiker.Factories
{
    public class EntityFactory : IEntityFactory
    {
        private readonly IDomainQuery _domainQuery;

        public EntityFactory(IDomainQuery domainQuery)
        {
            _domainQuery = domainQuery ?? throw new ArgumentNullException(nameof(domainQuery));
        }

        public User GetUser()
        {
            return new User(_domainQuery);
        }
    }
}
