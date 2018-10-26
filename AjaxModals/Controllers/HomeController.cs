using AjaxModals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AjaxModals.Controllers
{
    public class HomeController : Controller
    {
        private readonly static List<Contact> Contacts = new List<Contact>();

        public IActionResult Index()
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_Table", Contacts);
            }

            return View(Contacts);
        }

        public IActionResult Contact()
        {
            var model = new Contact { };

            return PartialView("_ContactModalPartial", model);
        }

        [HttpPost]
        public IActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                Contacts.Add(model);
                CreateNotification("Contact saved!");
            }

            return PartialView("_ContactModalPartial", model);
        }

        [NonAction]
        private void CreateNotification(string message)
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as List<string> ?? new List<string>();
            notifications.Add(message);
            TempData["Notifications"] = notifications;
        }

        public IActionResult Notifications()
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as IEnumerable<string> ?? Enumerable.Empty<string>();
            return PartialView("_NotificationsPartial", notifications);
        }
    }
}