using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PictureLiker.DAL.Repositories;

namespace PictureLiker.DAL
{
    public class Preference : EntityBase
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; private set; }

        public User User { get; private set; }

        [ForeignKey(nameof(Picture))]
        public int PictureId { get; private set; }

        public Picture Picture { get; private set; }

        [Required]
        public bool Like { get; private set; }
    }
}
