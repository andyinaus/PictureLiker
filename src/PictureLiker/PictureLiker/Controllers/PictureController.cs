using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLiker.Authentication;
using PictureLiker.DAL;
using PictureLiker.DAL.Entities;
using PictureLiker.Extensions;
using PictureLiker.Factories;
using PictureLiker.Models;

namespace PictureLiker.Controllers
{   //TODO: Unit Tests
    public class PictureController : Controller
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IUnitOfWork _unitOfWork;
        
        public PictureController(IUnitOfWork unitOfWork, IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var picture = GetNextAvailableRandomPicture();

            if (picture == null) return View();

            var model = new PictureModel
            {
                Id = picture.Id,
                PictureBytes = picture.Bytes
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(PictureModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (await IsPictureRated(model.Id))
            {
                return RedirectToAction("Index");
            }

            var preference = _entityFactory.GetPreference(User.GetUserId(), model.Id)
                .SetIsLiked(true);
            await _unitOfWork.PreferenceRepository.AddAsync(preference);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisLike(PictureModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (await IsPictureRated(model.Id))
            {
                return RedirectToAction("Index");
            }

            var preference = _entityFactory.GetPreference(User.GetUserId(), model.Id)
                .SetIsLiked(false);
            await _unitOfWork.PreferenceRepository.AddAsync(preference);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index");
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
        //If uploading large files or the uploading frequency is causing any resource problem, please consider to change the below implementation to upload file(s) 
        //via streaming. For more details, please refer to https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-2.2
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

                        var picture = _entityFactory.GetPicture().SetBytes(memoryStream.ToArray());
                        await _unitOfWork.PictureRepository.AddAsync(picture);
                    }
                }
            }

            await _unitOfWork.SaveAsync();

            return View("Upload");
        }

        private async Task<bool> IsPictureRated(int pictureId)
        {
            return await _unitOfWork.PreferenceRepository.FirstOrDefaultAsync(p => p.PictureId == pictureId && p.UserId == User.GetUserId()) != null;
        }

        private Picture GetNextAvailableRandomPicture()
        {
            return _unitOfWork.PictureRepository.FromSql(
                $"SELECT TOP 1 {nameof(Picture.Id)}, {nameof(Picture.Bytes)} " +
                "FROM (SELECT * FROM Pictures LEFT JOIN (SELECT PictureId FROM Preferences WHERE UserId = {0}) AS TEMP ON TEMP.PictureId = Id WHERE TEMP.PictureId IS NULL) AS AvailbalePictures " +
                "ORDER BY NEWID()", User.GetUserId())
                .FirstOrDefault();
        }

        public static class FileConstraints
        {
            public const int MaximumNumberOfFilesToBeUploaded = 10;
            public const long MaximumTotalFileSizeInBytes = 3 * 1024 * 1024 * MaximumNumberOfFilesToBeUploaded;
        }
    }
}