using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ECXBookApp.Business.Contracts;
using ECXBookApp.Entities.Models;
using ECXBookApp.Models;
using Newtonsoft.Json;
using ECXBookApp.Extensions;
using ECXBookApp.Filter;

namespace ECXBookApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataStore _store;

        public HomeController(ILogger<HomeController> logger, IDataStore bookManager)
        {
            _logger = logger;
            _store = bookManager;
        }

        [TypeFilter(typeof(UserSessionFilter))]
        public IActionResult Index()
        {
            try
            {
                var defaultUser = HttpContext.Session.Get<User>("UserDetail");
                TempData["User"] = defaultUser;

                var books = _store.GetUserAndBookRecord();
                var inventory = JsonConvert.SerializeObject(books.UserAndBookRecord);
                TempData["BookInventory"] = inventory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult BorrowBook(BooksViewModel model)
        {
            try
            {
                var updatedUserEntry = _store.BorrowBook(model.BooksData);
                HttpContext.Session.Set("UserDetail", updatedUserEntry);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ReturnBook(BooksViewModel model)
        {
            try
            {
                var entry = _store.ReturnBook(model.BooksData);
                HttpContext.Session.Set("UserDetail", entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
