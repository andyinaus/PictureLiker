using System.ComponentModel.DataAnnotations;

namespace PictureLiker.Models
{
    public class PictureModel
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        public byte[] PictureBytes { get; set; }
    }
}