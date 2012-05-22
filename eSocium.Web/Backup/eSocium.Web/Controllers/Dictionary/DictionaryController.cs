using System.Web.Mvc;

namespace eSocium.Web.Controllers.Dictionary
{
    /// <summary>
    /// Главный контроллер для подпроекта "Словарь терминов"
    /// </summary>
    [Authorize]
    public class DictionaryController : Controller
    {
        //
        // GET: /Dictionary/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
