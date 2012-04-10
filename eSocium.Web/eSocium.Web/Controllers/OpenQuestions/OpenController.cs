using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eSocium.Web.Controllers.OpenQuestions
{
    /// <summary>
    /// Главный контроллер для подпроекта "Обработка открытых вопросов"
    /// </summary>
    [Authorize]
    public class OpenController : Controller
    {
        //
        // GET: /Open/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
