using RedisWeb.Services;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace RedisWeb.Controllers
{
    public class HomeController : BaseController
    {
     
        public ActionResult Index()
        {
            var data= new IndexService().GetBlogData();
            ViewBag.Data = data;
            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            var data = new IndexService().GetBlogData();
            data=data.Skip(100000).Take(100);
            return Json(data);
        }
     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}