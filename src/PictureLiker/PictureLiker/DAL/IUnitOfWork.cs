﻿using System.Threading.Tasks;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    public interface IUnitOfWork
    {
        IRepository<User> UseRepository { get; }
        IRepository<Picture> PictureRepository { get; }
        void Save();
        Task SaveAsync();
    }
}