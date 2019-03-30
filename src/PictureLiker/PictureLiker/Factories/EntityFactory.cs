﻿using System;
using PictureLiker.DAL;
using PictureLiker.Services;

namespace PictureLiker.Factories
{
    //TODO: If number of services needed to be injected to entities incresed gradually, consider using other dependency injection library like autofac to resolve the entities from Ioc 
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
