using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

using PagedList;

namespace WedSiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int PageSize = 6;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            //Tim kiems theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n =>n.TenSP).ToPagedList(PageNumber, PageSize));
        }

        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
          //Gọi hàm get tìm kiếm
            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa});
        }
        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            //Tim kiems theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(n=>n.DonGia));
        }
    }
}