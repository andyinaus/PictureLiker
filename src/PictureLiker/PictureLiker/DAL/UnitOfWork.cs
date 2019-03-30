using System;
using System.Threading.Tasks;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PictureLikerContext _dbContext;
        private IRepository<User> _userRepository;
        private IRepository<Picture> _pictureRepository;

        public UnitOfWork(PictureLikerContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<User> UseRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new Repository<User>(_dbContext);
                }

                return _userRepository;
            }
        }

        public IRepository<Picture> PictureRepository
        {
            get
            {
                if (_pictureRepository == null)
                {
                    _pictureRepository = new Repository<Picture>(_dbContext);
                }

                return _pictureRepository;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}