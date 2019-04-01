using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PictureLiker.DAL.Entities
{
    public class Preference : EntityBase
    {
        private Preference() { }

        public Preference(int userId, int pictureId)
        {
            UserId = userId;
            PictureId = pictureId;
        }

        [ForeignKey(nameof(User))]
        public int UserId { get; private set; }

        public User User { get; private set; }

        [ForeignKey(nameof(Picture))]
        public int PictureId { get; private set; }

        public Picture Picture { get; private set; }

        [Required]
        public bool IsLiked { get; private set; }

        public Preference SetIsLiked(bool isLiked)
        {
            IsLiked = isLiked;

            return this;
        }
    }
}
