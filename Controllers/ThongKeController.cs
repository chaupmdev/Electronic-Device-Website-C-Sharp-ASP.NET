using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WedSiteBanHang.Models;
using System.Web.Mvc;

namespace WedSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString(); ;//Lấy số lượng truy cấp từ application đã được tạo 
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString(); ;//Lấy số lượng truy cấp từ application đã được tạo 
            ViewBag.TongDoanhThu = ThongKeDoanhThu(); //Thống kê tổng doanh thu
            ViewBag.TongDonDatHang = ThongKeDonHang();//Thống kê đơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien(); //Thống kê thành viên

            return View();
        }
        public double ThongKeDonHang()
        {
            //Dem đơn đặt hàng
            double ddh = db.DonDatHangs.Count();
            return ddh;
        }
        public double ThongKeThanhVien()
        {
            //Dem đơn đặt hàng
            double sltv = db.ThanhViens.Count();
            return sltv;
        }
        public decimal ThongKeDoanhThu()
        {
            //Thống kê theo tất cả doanh thu từ khi wedsite thành lập
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }

        public decimal ThongKeTongDoanhThuThang(int thang, int nam)
        {
            //Thống kê theo tất cả doanh thu theo tháng
            //List ra những đơn đặt hàng nào có tháng năm tương ứng
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
            decimal TongTien = 0;
            //Duyệt chi tiết đơn đặthàng đó và lấy tổng tiền của tất cả các sản phảm thuộc hàng đơn hàng đó
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien;
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