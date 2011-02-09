using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FizzWare.NBuilder;
using NetCommunityEvents.Controllers;
using NetCommunityEvents.Data;
using NetCommunityEvents.Infrastructure.Auth;
using NetCommunityEvents.Infrastructure.Extensions;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetCommunityEvents.Tests.Controllers
{
    public class AuthControllerTest
    {
        protected IDataRepository<User> DataRepository;
        protected IAuthService AuthService;
        protected AuthController Sut;

        protected IEnumerable<User> Users;

        [SetUp]
        public void Setup()
        {
            Users = Builder<User>.CreateListOfSize(50)
                .WhereAll()
                    .Have(u => u.Password = u.Password.ConvertToHash())
                    .And(u => u.Role = Role.Member)
                    .And(u => u.Email = String.Format("{0}@domain.com", u.Email))
                .WhereTheFirst(5)
                    .Have(u => u.Role = Role.Admin)
                .WhereTheLast(5)
                    .Have(u => u.Role = Role.Guest)
                .Build();

            DataRepository = MockRepository.GenerateStub<IDataRepository<User>>();
            AuthService = MockRepository.GenerateStub<IAuthService>();
            Sut = new AuthController(DataRepository, AuthService);
        }

        [Test]
        public void Index()
        {
            // Arrange
            DataRepository
                .Stub(d => d.SelectEntities(a => true))
                .Return(Users.Where(a => true))
                .IgnoreArguments();

            // Act
            var viewResult = Sut.Index() as ViewResult;
            var viewModel = viewResult.Model as UserListViewModel;

            // Assert
            Assert.That(viewModel.Users, Is.Not.Null);
            Assert.That(viewModel.Users.Count(), Is.EqualTo(50));
            Assert.That(viewModel.AllUsersLength, Is.GreaterThanOrEqualTo(50));
        }

        [Test]
        public void Login_User_exists()
        {
            // Arrange
            var viewModel = new LoginViewModel
                                {
                                    Email = "Email6@domain.com",
                                    Password = "Password6",
                                    Persistent = true
                                };
            DataRepository.Stub(
                d => d.SelectEntity(u => u.Email == viewModel.Email && u.Password == viewModel.Password.ConvertToHash()))
                .IgnoreArguments()
                .Return(
                    Users.Where(u => u.Email == viewModel.Email && u.Password == viewModel.Password.ConvertToHash()).
                        FirstOrDefault());

            // Act
            var result = Sut.Login(viewModel) as RedirectToRouteResult;

            var controllerName = result.RouteValues["controller"];
            var actionName = result.RouteValues["action"];
            var modelId = result.RouteValues["id"];

            // Assert
            Assert.That(controllerName, Is.Not.Null.Or.Empty);
            Assert.That(controllerName, Is.EqualTo("Auth"));
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Details"));
            Assert.That(modelId, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void Login_User_not_exists()
        {
            // Arrange
            var viewModel = new LoginViewModel
                                {
                                    Email = "mail@domain.com",
                                    Password = "secret",
                                    Persistent = true
                                };
            DataRepository.Stub(
                d => d.SelectEntity(u => u.Email == viewModel.Email && u.Password == viewModel.Password.ConvertToHash()))
                .Return(
                    Users.Where(u => u.Email == viewModel.Email && u.Password == viewModel.Password.ConvertToHash()).
                        FirstOrDefault());

            // Act
            var result = Sut.Login(viewModel) as RedirectToRouteResult;

            var controllerName = result.RouteValues["controller"];
            var actionName = result.RouteValues["action"];
            var modelId = result.RouteValues["id"];

            // Assert
            Assert.That(controllerName, Is.Not.Null.Or.Empty);
            Assert.That(controllerName, Is.EqualTo("Content"));
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Show"));
            Assert.That(modelId, Is.Not.Null.Or.Empty);
            Assert.That(modelId, Is.EqualTo("LoginError"));
        }

        [Test]
        public void Logout()
        {
            // Arrange
           
            // Act
            var result = Sut.Logout() as RedirectToRouteResult;

            var controllerName = result.RouteValues["controller"];
            var actionName = result.RouteValues["action"];

            // Assert
            Assert.That(controllerName, Is.Not.Null.Or.Empty);
            Assert.That(controllerName, Is.EqualTo("Home"));
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Index"));
        }

        [Test]
        public void Details()
        {
            // Arrange
            var userId = new Guid("00000000-0000-0000-0000-000000000001");
            var user = new User
                           {
                               Id = userId
                           };

            DataRepository.Stub(d => d.SelectEntity(u => u.Id == userId))
                .Return(user)
                .IgnoreArguments();

            // Act
            var viewResult = Sut.Details(userId) as ViewResult;
            var viewModel = viewResult.Model as UserViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.EqualTo(userId));
        }

        [Test]
        public void Create_Get()
        {
            // Arrange
           
            // Act
            var viewResult = Sut.Create() as ViewResult;
            var viewModel = viewResult.Model as UserViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void Create_Post()
        {
            // Arrange
            var viewModel = new UserViewModel
                                {
                                    Password = "topsecret",
                                    Email = "email@domain.com"
                                };

            // Act
            var result = Sut.Create(viewModel) as RedirectToRouteResult;

            var controllerName = result.RouteValues["controller"];
            var actionName = result.RouteValues["action"];
            var modelId = result.RouteValues["id"];

            // Assert
            Assert.That(controllerName, Is.Not.Null.Or.Empty);
            Assert.That(controllerName, Is.EqualTo("Content"));
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Show"));
            Assert.That(modelId, Is.Not.Null.Or.Empty);
            Assert.That(modelId, Is.EqualTo("CreateMemberSuccess"));
        }

        [Test]
        public void Edit_Get()
        {
            // Arrange
            var userId = new Guid("00000000-0000-0000-0000-000000000001");
            var user = new User
                           {
                               Id = userId
                           };

            DataRepository.Stub(d => d.SelectEntity(u => u.Id == userId))
                .Return(user)
                .IgnoreArguments();

            // Act
            var viewResult = Sut.Edit(userId) as ViewResult;
            var viewModel = viewResult.Model as UserViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.EqualTo(userId));
        }
    }
}
