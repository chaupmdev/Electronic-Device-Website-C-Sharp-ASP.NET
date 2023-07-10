using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuyenController : Controller
    {
        // GET: Quyen
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.Quyens.OrderBy(n=>n.TenQuyen));
        }
        [HttpGet]
        public ActionResult ThemQuyen()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemQuyen(Quyen quyen)
        {
            if(ModelState.IsValid)
            {
                db.Quyens.Add(quyen);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // Hiển thị quyền cần sửa
        [HttpGet]
        public ActionResult ChinhSuaQuyen(string id)
        {
            //
           
            Quyen quyen = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            
            //Load dropdowlist nhà cung cấp
            ViewBag.MaQuyen = quyen.MaQuyen;
            //Load dropdowlist loai sản phẩm
            ViewBag.TenQuyen = quyen.TenQuyen;
            //Load dropdowlist Nhà sản xuất
          
            return View(quyen);
        }
        [HttpPost]
        //Cập nhật quyền
        public ActionResult ChinhSuaQuyen(Quyen updatequyen)
        {
            //Truy vấn lấy ra dữ liệu của đơn hàng đó'
            Quyen quyenUpdate = db.Quyens.Single(n => n.MaQuyen == updatequyen.MaQuyen);
            quyenUpdate.TenQuyen = updatequyen.TenQuyen;
            db.SaveChanges();
            return RedirectToAction("Index","Quyen");
        }
        //Xem chi tiết
        public ActionResult XemChiTiet(string id)
        {
           
            // Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            Quyen xemquyen = db.Quyens.SingleOrDefault(n => n.MaQuyen == id );
           
            return View(xemquyen);
        }
       
        public ActionResult Xoa(string id)
        {
            
            Quyen xoaQuyen = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            
            db.Quyens.Remove(xoaQuyen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}