using System.ComponentModel.DataAnnotations;

namespace PictureLiker.Models
{
    public class SearchCriteria
    {
        [Range(1, int.MaxValue)]
        public int RequestedPage { get; set; }
    }
}