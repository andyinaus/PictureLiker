using System;
using System.Threading.Tasks;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    //TODO: Unit Tests
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PictureLikerContext _dbContext;
        private IRepository<User> _userRepository;
        private IRepository<Picture> _pictureRepository;
        private IRepository<Preference> _preferenceRepository;

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

        public IRepository<Preference> PreferenceRepository
        {
            get
            {
                if (_preferenceRepository == null)
                {
                    _preferenceRepository = new Repository<Preference>(_dbContext);
                }

                return _preferenceRepository;
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