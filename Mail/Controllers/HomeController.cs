using Mail.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mail.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Send(MailModel model)
        {
            string json; ;
            if (ModelState.IsValid)
            {
                // TODO: хранить id автора в сессии

                User author = ORM.SearchUser(User.Identity.Name);
                User target = ORM.SearchUser(model.Target);
                
                if (target == null)
                {
                    json = JsonSerializer.Serialize<string>("Нет такого пользователя");
                    return json;
                }
                model.AuthorId = author.Id;
                model.TargetId = target.Id;

                // отправить сообщение в базу
                ORM.CreateMail(model);

                // вернуть строку в javascipt
                json = JsonSerializer.Serialize<string>("Сообщение отправлено");
                return json;
            }
            else
            {
                json = JsonSerializer.Serialize<string>("Ошибки при заполнении");
                return json;
            }
               
        }

        /*
        public IActionResult Privacy()
        {
            return View();
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
