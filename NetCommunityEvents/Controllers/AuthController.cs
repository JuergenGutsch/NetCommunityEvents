using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetCommunityEvents.Data;
using NetCommunityEvents.Infrastructure.Auth;
using NetCommunityEvents.Infrastructure.Extensions;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;

namespace NetCommunityEvents.Controllers
{
    [HandleError]
    public class AuthController : Controller
    {

        private readonly IDataRepository<User> _dataRepository;
        private readonly IAuthService _authService;

        public AuthController(IDataRepository<User> dataRepository, IAuthService authService)
        {
            _dataRepository = dataRepository;
            _authService = authService;
        }

        [IsAuthenticated]
        public ActionResult Index()
        {
            IEnumerable<User> users = _dataRepository.SelectEntities(u => true);

            var viewModel = new UserListViewModel
                                {
                                    Users = users.Select(u => UserViewModel.Create(u)),
                                    AllUsersLength = users.Count()
                                };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            User user = _dataRepository.SelectEntity(u => u.Email == viewModel.Email && u.Password == viewModel.Password.ConvertToHash());
            if (user != null)
            {
                _authService.SignIn(user.Name, viewModel.Persistent);
                return this.RedirectToActionWithId("Auth", "Details", user.Id);
            }
            return this.RedirectToActionWithId("Content", "Show", "LoginError");
        }

        public ActionResult Logout()
        {
            _authService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //public ActionResult NewPassword()
        //{
        //    ViewData["Status"] = false;
        //    return View();
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult NewPassword(UserViewModel viewModel)
        //{
        //    ViewData["Status"] = false;
        //    try
        //    {
        //        string passwd = Encrypt.CreateNewPassword();

        //        User user = new User();
        //        UpdateModel(user, new[] { "Email" });

        //        user = _dataRepository.SelectEntity(u => u.Email == viewModel.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError("Email", "Ein Benutzer mit dieser E-Mailadresse ist uns nicht bekannt.");
        //            return View();
        //        }

        //        user.Password = passwd.ConvertToHash();

        //        if (!userService.SaveUser(user))
        //        {
        //            return View();
        //        }

        //        //sendmailService.Sender = new MailAddress("info@dotnetkk.de", ".NET Stammtisch");
        //        //sendmailService.Sendmail(
        //        //    new MailAddress(user.Email),
        //        //    NewpasswordSubject,
        //        //    String.Format(NewpasswordMailText, passwd));


        //        ViewData["Message"] = "Das neue Passwort wurde Ihnen zugesandt.";
        //        ViewData["Status"] = true;
        //        return View();

        //    }
        //    catch (Exception)
        //    {
        //        return View();
        //    }

        //}

        //[IsAuthenticated]
        //public ActionResult ChangePassword()
        //{
        //    User currentUser = ((CustomIdentity)HttpContext.User.Identity).User;
        //    ViewData["Status"] = false;
        //    return View(currentUser);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //[IsAuthenticated]
        //public ActionResult ChangePassword(FormCollection collection)
        //{
        //    User currentUser = ((CustomIdentity)HttpContext.User.Identity).User;
        //    ViewData["Status"] = false;
        //    try
        //    {
        //        if (String.IsNullOrEmpty(collection["oldPassword"]))
        //        {
        //            ModelState.AddModelError("oldPassword", "Sie m?ssen Ihr altes Passwort angeben.");
        //        }

        //        if (currentUser.Password != collection["oldPassword"].ConvertToHash())
        //        {
        //            ModelState.AddModelError("oldPassword", "Ihr altes Passwort stimmt nicht.");
        //        }

        //        if (String.IsNullOrEmpty(collection["newPassword1"]))
        //        {
        //            ModelState.AddModelError("newPassword1", "Das Feld Passwort darf nicht leer sein.");
        //        }

        //        if (String.IsNullOrEmpty(collection["newPassword2"]))
        //        {
        //            ModelState.AddModelError("newPassword2", "Die Passwortwiederholung darf nicht leer sein.");
        //        }

        //        if (collection["newPassword1"] != collection["newPassword2"])
        //        {
        //            ModelState.AddModelError("newPassword2", "Das Feld Passwort stimmt nicht mit der Passwortwiederholung ?berein.");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return View();
        //        }

        //        currentUser.Password = collection["newPassword1"].ConvertToHash();

        //        if (!userService.SaveUser(currentUser))
        //        {
        //            return View();
        //        }

        //        sendmailService.Sender = new MailAddress("info@dotnetkk.de", ".NET Stammtisch");
        //        sendmailService.Sendmail(
        //            new MailAddress(currentUser.Email),
        //            ChangedPasswordSubject,
        //            String.Format(ChangedPasswordMailText, collection["newPassword1"]));

        //        ViewData["Message"] = "Ihr Passwort wurde ge?ndert.";
        //        ViewData["Status"] = true;
        //    }
        //    catch (Exception)
        //    {
        //        return View();
        //    }

        //    return View(currentUser);
        //}

        [IsAuthenticated]
        public ActionResult Details(Guid id)
        {
            var user = _dataRepository.SelectEntity(u => u.Id == id);

            var viewModel = UserViewModel.Create(user);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Create(UserViewModel viewModel)
        {
            var user = viewModel.CreateModel();
            user.Password = user.Password.ConvertToHash();
            user.IsValid = false;
            user.Role = Role.Member;

            _dataRepository.SaveEntity(user);

            //sendmailService.Sender = new MailAddress("info@dotnetkk.de", ".NET Stammtisch");
            //sendmailService.Sendmail(
            //    new MailAddress(user.Email),
            //    RegisterSubject,
            //    String.Format(
            //        RegisterMailText,
            //        this.Action("Member", "Validate"),
            //        user.Id,
            //        HttpContext.Request.Url.Host));

            return this.RedirectToActionWithId("Content", "Show", "CreateMemberSuccess");
        }

        public ActionResult Validate(Guid id)
        {
            var user = _dataRepository.SelectEntity(u => u.Id == id);
            if (user == null)
            {
                ViewData["Message"] = "Fer Freischaltlink ist ung?ltig.";
                return this.RedirectToActionWithId("Content", "Show", "FreischaltlinkNotValid");
            }
            if (user.IsValid)
            {
                ViewData["Message"] = "Der Account ist bereits freigeschalten.";
                return this.RedirectToActionWithId("Content", "Show", "AcountAllrearyFreigeschalten");
            }
            user.IsValid = true;

            _dataRepository.SaveEntity(user);

            ViewData["Message"] = "Der Account wurde erfolgreich freigeschalten. Sie k?nnen sich nun anmelden.";
            return this.RedirectToActionWithId("Content", "Show", "SucccessfullFreigeschalten");
        }

        [HttpGet]
        [IsAuthenticated]
        public ActionResult Edit(Guid id)
        {
            var user = _dataRepository.SelectEntity(u => u.Id == id);

            var viewModel = UserViewModel.Create(user);

            return View(viewModel);
        }

        [HttpPost]
        [IsAuthenticated]
        public ActionResult Edit(Guid id, UserViewModel viewModel)
        {
            var model = viewModel.CreateModel();

            _dataRepository.SaveEntity(model);

            return View(model);
        }

        [UserInRole(Role.Admin)]
        public ActionResult Delete(Guid id)
        {
            _dataRepository.DelelteEntity(u => u.Id == id);

            return RedirectToAction("Index");
        }


    }
}
