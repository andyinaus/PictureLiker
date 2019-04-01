using System.ComponentModel.DataAnnotations;

namespace PictureLiker.Models
{
    public class PreferencesModel
    {
        //TODO: Consider making 'below const' configurable, ways: appsettings or database
        public const int DefaultRecordsPerPage = 10;

        public const int DefaultMaximumPagesDisplayed = 6;

        public PreferencesModel()
        {
            SearchCriteria = new SearchCriteria
            {
                RequestedPage = 1
            };
        }

        public DataWithPagination<PicturePreference> PreferencesWithPagination { get; set; }

        [Required]
        public SearchCriteria SearchCriteria { get; set; }
    }

    public class PicturePreference
    {
        public int PictureId { get; set; }
        public bool IsLiked { get; set; }
        public byte[] PictureBytes { get; set; }
    }
}