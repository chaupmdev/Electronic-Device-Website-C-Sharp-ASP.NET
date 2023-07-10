using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WedSiteBanHang.Controllers
{
    [Authorize(Roles = "DangKy")]
    public class DangKyController : Controller
    {
        // GET: DangKy
        public ActionResult Index()
        {
            return View();
        }
    }
}