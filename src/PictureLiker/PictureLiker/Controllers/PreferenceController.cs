using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLiker.DAL;
using PictureLiker.Extensions;
using PictureLiker.Models;

namespace PictureLiker.Controllers
{   //TODO: Unit Tests
    public class PreferenceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PreferenceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(PreferencesModel model = null)
        {
            model = model ?? new PreferencesModel();

            var userId = User.GetUserId();
            var preferences = _unitOfWork.PreferenceRepository.TakeAtPage(model.SearchCriteria.RequestedPage, PreferencesModel.DefaultRecordsPerPage, p => p.UserId == userId);

            var preferencesWithPicture = preferences.ToList().Select(p => new PicturePreference
            {
                PictureId = p.PictureId,
                IsLiked = p.IsLiked,
                PictureBytes = _unitOfWork.PictureRepository.FirstOrDefault(pic => pic.Id == p.PictureId)?.Bytes
            }).ToArray();

            return View(new PreferencesModel
            {
                PreferencesWithPagination = new DataWithPagination<PicturePreference>
                {
                    ItemsAtCurrentPage = preferencesWithPicture,
                    Page = model.SearchCriteria.RequestedPage,
                    RecordsPerPage = PreferencesModel.DefaultRecordsPerPage,
                    TotalItems = _unitOfWork.PreferenceRepository.LongCount(p => p.UserId == userId)
                }
            });
        }
    }
}