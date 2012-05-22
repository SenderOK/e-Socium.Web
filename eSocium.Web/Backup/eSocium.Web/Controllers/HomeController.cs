using System.Web.Mvc;
using eSocium.Web.Models.DAL;

namespace eSocium.Web.Controllers
{
    /// <summary>
    /// Главный контроллер сайта, который содержит в себе информационные страницы и ссылки на 
    /// подпроекты
    /// </summary>
    public class HomeController : BaseController
    {
        UserContext db = new UserContext();

        public ActionResult Index()
        {
            ViewBag.User = User.Identity.Name;
            return View();
        }
    }
}
