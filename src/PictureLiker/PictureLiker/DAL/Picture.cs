using System;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    public class Picture : EntityBase
    {
        public Picture SetBytes(byte[] bytes)
        {
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

            return this;
        }

        public byte[] Bytes { get; private set; }
    }
}