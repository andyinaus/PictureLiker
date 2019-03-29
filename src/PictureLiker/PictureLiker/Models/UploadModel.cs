using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PictureLiker.Models
{
    public class UploadModel
    {
        [Required]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
