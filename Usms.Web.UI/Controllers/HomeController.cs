using System.Web.Mvc;

namespace Usms.Web.UI.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Hello()
		{
			return View();
		}
	}
}
