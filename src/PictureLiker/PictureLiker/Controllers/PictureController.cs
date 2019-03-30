﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLiker.Authentication;
using PictureLiker.DAL;
using PictureLiker.Extensions;
using PictureLiker.Models;

namespace PictureLiker.Controllers
{
    public class PictureController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public PictureController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var picture = await _unitOfWork.PictureRepository.FirstOrDefaultAsync();

            if (picture == null) return View();

            var model = new PictureModel
            {
                Id = picture.Id,
                PictureBytes = picture.Bytes
            };

            return View(model);
        }

        public async Task<IActionResult> Like(PictureModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (await IsPictureLiked(model.Id))
            {
                return RedirectToAction("Index");
            }

            var preference = new Preference(User.GetUserId(), model.Id);
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
                        await _unitOfWork.PictureRepository.AddAsync(picture);
                    }
                }
            }

            await _unitOfWork.SaveAsync();

            return View("Upload");
        }

        private async Task<bool> IsPictureLiked(int pictureId)
        {
            return await _unitOfWork.PreferenceRepository.FirstOrDefaultAsync(p => p.PictureId == pictureId && p.UserId == User.GetUserId()) != null;
        }

        public static class FileConstraints
        {
            public const int MaximumNumberOfFilesToBeUploaded = 10;
            public const long MaximumTotalFileSizeInBytes = 3 * 1024 * 1024 * MaximumNumberOfFilesToBeUploaded;
        }
    }
}