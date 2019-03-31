using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLiker.DAL;
using PictureLiker.Extensions;
using PictureLiker.Models;

namespace PictureLiker.Controllers
{
    public class PreferenceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PreferenceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            //TODO: pagination 
            var preferences = _unitOfWork.PreferenceRepository.TakeAtPage(1, 10, p => p.UserId == User.GetUserId());

            var preferencesWithPicture = preferences.ToList().Select(p => new PicturePreference
            {
                PictureId = p.PictureId,
                IsLiked = p.IsLiked,
                PictureBytes = _unitOfWork.PictureRepository.FirstOrDefault(pic => pic.Id == p.PictureId)?.Bytes
            }).ToList();

            return View(new PreferencesModel
            {
                Preferences = preferencesWithPicture
            });
        }
    }
}