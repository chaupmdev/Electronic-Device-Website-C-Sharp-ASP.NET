using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [Authorize(Roles = "QuanLySanPham")]
    
        public ActionResult Index()
        {
            // Cách 1Lấy dữ liệu là danh sách khách hàng
            //var lstKH = from KH in db.KhachHangs select KH;
            // Cách 2: Dùng có sẵn
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
        [Authorize(Roles = "QuanTri")]
        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHangs select KH;
            return View(lstKH);
        }

        public ActionResult TruyVan1DoiTuong()
        {
            // Cách 1: Truy vấn đối tượng bằng câu lệnh truy vấn
            // Bước 1: Lấy danh sách khách hàng
            var lstKH = from KH in db.KhachHangs where KH.MaKH == 1 select KH;
            // Bước 2: Lấy đối tượng khách hàng
            //KhachHang khang = lstKH.FirstOrDefault();
            //Cách 2: Lấy đối tượng dựa trên phương thức hỗ trợ
            KhachHang khang = db.KhachHangs.SingleOrDefault(n=>n.MaKH==1);
            return View(khang);
        }
        public ActionResult SortDulieu()
        {
            // Phương thức sắp xếp dữ liệu
            List<KhachHang> lstKH = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
            return View(lstKH);
        }
        public ActionResult GroupDulieu()
        {
            //Group dữ liệu
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}