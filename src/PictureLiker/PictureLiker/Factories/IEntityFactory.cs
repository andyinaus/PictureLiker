﻿using PictureLiker.DAL.Entities;

namespace PictureLiker.Factories
{
    public interface IEntityFactory
    {
        User GetUser();
        Preference GetPreference(int userId, int pictureId);
        Picture GetPicture();
    }
}