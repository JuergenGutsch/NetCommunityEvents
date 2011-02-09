using System;
using System.Web.Mvc;
using System.Web.Security;
using NetCommunityEvents.Data;
using NetCommunityEvents.Infrastructure.Auth;
using NetCommunityEvents.Infrastructure.Extensions;
using NetCommunityEvents.Models;

namespace NetCommunityEvents.Controllers
{
    [HandleError]
    public class ContentController : Controller
    {
        private readonly IDataRepository<Content> _dataRepository;

        public ContentController(IDataRepository<Content> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public ActionResult Show(Guid id)
        {
            var model = _dataRepository.SelectEntity(c => c.Id == id);

            var viewModel = ContentViewModel.Create(model);

            viewModel.ShowEditLink = this.IsInRole(Role.Admin);

            return View(viewModel);
        }

        private static ContentViewModel NewContent(string name, string value)
        {
            return new ContentViewModel
                       {
                           Name = name,
                           PageTitle = value,
                           PageContent = value
                       };
        }

        [UserInRole(Role.Admin)]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var model = _dataRepository.SelectEntity(c => c.Id == id);

            var viewModel = ContentViewModel.Create(model);

            return View(viewModel);
        }

        [UserInRole(Role.Admin)]
        [HttpPost]
        public ActionResult Edit(Guid id, ContentViewModel viewModel)
        {
            var model = viewModel.CreateModel();

            _dataRepository.SaveEntity(model);

            return this.RedirectToActionWithId("Content", "Show", id);
        }

        [UserInRole(Role.Admin)]
        public ActionResult Delete(Guid id)
        {
            _dataRepository.DelelteEntity(c => c.Id == id);

            return RedirectToAction("Index", "Home");
        }

    }

    public class ContentViewModel
    {
        public static ContentViewModel Create(Content model)
        {
            throw new NotImplementedException();
        }

        public bool ShowEditLink { get; set; }

        public string Name { get; set; }

        public string PageTitle { get; set; }

        public string PageContent { get; set; }

        public Content CreateModel()
        {
            throw new NotImplementedException();
        }
    }
}
