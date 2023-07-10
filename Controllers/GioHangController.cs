using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //Lay gio hang
        public List<ItemGioHang> LayGioHang()
        {
            // Gio hang da ton tai
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                //Neu session gio hang khong ton tai thi tao gio hang
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
               
            }
            return lstGioHang;
        }
        // Them gio hang thong thuong (load tai trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiem tra san pham co ton tai trong CSDL thanh cong khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
                if (sp == null)
            {
                //Trang dan khong hop le
                Response.StatusCode = 404;
                return null;
            }

            // Lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            // Truong hop 1 neu san pham ton tai trong gio hang
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck != null)
            {
                //Kiem tra so luong truoc khi cho khach hang mua hang
                if(sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;

            if(lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        //Tinh tong tien
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;

            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        //
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
          
        }
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TinhTongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TinhTongTien = TinhTongTien();
            return PartialView();
        }
        // Chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra session gio hang ton tai chua
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Kiem tra san pham co ton tai trong CSDL hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list gio hang tu Session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiem tra xem san pham do co ton tai trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            // nếu tồn tại
            return View(spCheck);
        }
        // Xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong session giỏ hàng
            //Bước 1: lấy list<GioHang> từ Session["GioHang"]

            List<ItemGioHang> lstGH = LayGioHang();
            //Bước 2: lấy sản phẩm cần cập nhật từ trong listGioHang ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Bước 3: Tiến hành cập nhật lại số lượng cũng như thành tiền
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("SuaGioHang", new { @MaSP = itemGH.MaSP });

        }
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Kiem tra san pham co ton tai trong CSDL hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list gio hang tu Session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiem tra xem san pham do co ton tai trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Xóa item trong gio hang
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang", "GioHang");
        }

        //Xây dựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            //Kiểm tra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if(Session["TaiKhoan"] == null)
            {
                //Thêm khách hàng vào bảng khách hàng đối với khách hàng vãng lai
                khang = new KhachHang();
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //Đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }

            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            // thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang", "GioHang");
        }

    }
}