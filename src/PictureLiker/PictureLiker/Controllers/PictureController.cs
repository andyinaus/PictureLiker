using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLiker.Authentication;
using PictureLiker.DAL;
using PictureLiker.DAL.Repositories;
using PictureLiker.Models;

namespace PictureLiker.Controllers
{
    public class PictureController : Controller
    {
        private readonly IRepository<Picture> _pictureRepository;
        
        public PictureController(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository ?? throw new ArgumentNullException(nameof(pictureRepository));
            
        }

        [HttpGet]
        [Authorize(Roles = RoleTypes.Administrator)]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.Administrator)]
        [RequestSizeLimit(FileConstraints.MaximumTotalFileSizeInBytes)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAsync(UploadModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.Files == null) throw new ArgumentNullException(nameof(model.Files));

            if (model.Files.Count() > FileConstraints.MaximumNumberOfFilesToBeUploaded)
            {
                ModelState.AddModelError(nameof(model.Files), $"Maximum number of files you can upload is {FileConstraints.MaximumNumberOfFilesToBeUploaded} each time.");

                if (!ModelState.IsValid) return View(nameof(Upload), model);

                return View("Upload", model);
            }

            foreach (var formFile in model.Files)
            {
                if (formFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);

                        var picture = new Picture().SetBytes(memoryStream.ToArray());
                        await _pictureRepository.AddAsync(picture);
                    }
                }
            }

            await _pictureRepository.SaveAsync();

            return View("Upload");
        }

        public static class FileConstraints
        {
            public const int MaximumNumberOfFilesToBeUploaded = 10;
            public const long MaximumTotalFileSizeInBytes = 3 * 1024 * 1024 * MaximumNumberOfFilesToBeUploaded;
        }
    }
}