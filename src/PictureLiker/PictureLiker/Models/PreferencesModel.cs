using System.Collections.Generic;

namespace PictureLiker.Models
{
    public class PreferencesModel
    {
        public IList<PicturePreference> Preferences { get; set; }
    }

    public class PicturePreference
    {
        public int PictureId { get; set; }
        public bool IsLiked { get; set; }
        public byte[] PictureBytes { get; set; }
    }
}