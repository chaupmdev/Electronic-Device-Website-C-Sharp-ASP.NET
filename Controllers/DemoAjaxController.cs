using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    public class DemoAjaxController : Controller
    {
        // GET: DemoAjax
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult DemoAjax()
        {
           
            return View();
        }
        // Xu ly ajax action link
        public ActionResult LoadAjaxActionLink( )
        {
            System.Threading.Thread.Sleep(2000);
            return Content("Hello Ajax");
        }
        public ActionResult LoadAjaxBeginForm(FormCollection f)
        {
            System.Threading.Thread.Sleep(2000);
            string KQ = f["txt1"].ToString();
            return Content(KQ);
        }
        // xu ly ajax jquery
        public ActionResult LoadAjaxJQuery(int a, int b)
        {
            System.Threading.Thread.Sleep(2000);
            return Content((a + b).ToString());
        }
        // Su dung load ajax tra ve ket qua 1 partialview
      
        public ActionResult LoadSanphamAjax()
        {
            var LstSanpham = db.SanPhams;
            return PartialView(LstSanpham);
        }
    }
    
}