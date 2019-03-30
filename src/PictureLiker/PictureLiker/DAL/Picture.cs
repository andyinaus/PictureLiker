using System;
using System.ComponentModel.DataAnnotations;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    public class Picture : EntityBase
    {
        private Picture() { }

        public Picture SetBytes(byte[] bytes)
        {
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

            return this;
        }

        [Required]
        public byte[] Bytes { get; private set; }
    }
}