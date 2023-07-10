using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WedSiteBanHang.Controllers
{
    
    public class HomeController : Controller
    {
        
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Home
        public ActionResult Index()
        {
            // Lan luot tao cac viewbag de lay list sanpham tu co so du lieu
            //List dien thoia moi nhat

            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListDTM = lstDTM;
            //  Gan vao viewbag
            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListLTM = lstLTM;


            var lstMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);

            ViewBag.ListMTB = lstMTB;
            
            return View();
        }

        public ActionResult MenuParial()
        {
            // Truy vấn lấy về 1 list sản phẩm
            var LstSP = db.SanPhams;
          
            return PartialView(LstSP);
        }
        public ActionResult ThanhtraiPatial()
        {
            // Truy vấn lấy về 1 list sản phẩm
            var LstSP = db.SanPhams;
            
            var lstBanChayNhat = db.SanPhams.Where(n => n.SoLanMua > 0).Max(p => p.SoLanMua);
            ViewBag.ListBCN = lstBanChayNhat;
            return PartialView(LstSP);
        }
       
         
          
        [HttpGet]
        public ActionResult Dangky()

        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }


        [HttpPost]
        public ActionResult Dangky(ThanhVien tv, FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //Kiểm tra captcha hợp lệ
            if (this.IsCaptchaValid("Captcha không hợp lệ"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Thêm thành công";
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ThongBao = "Thêm thất bại";
                }
                return View();
            }
            //Thêm khách hàng vào csdl
            ViewBag.ThongBao = "Sai mã captcha";


            return View();
        }

        [HttpGet]
        public ActionResult Dangky1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky1(ThanhVien tv)
        {
            return View();
        }
        // Load câu hỏi bí mật
        public List<string> LoadCauHoi()
        {
           
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            return lstCauHoi;
        }
        //Xây dựng action đăng nhập
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            
            //Kiểm tra đăng nhập mật khẩu
            //string sTaiKhoan = f["txtTenDangNhap"].ToString();
            //string sMatKhau = f["txtMatKhau"].ToString();
            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            //if (tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    return Content("<script>window.location.reload();</script>");
            //}
            //return Content("Tài khoản hoặc mật khẩu không đúng");
            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            //Truy vấn kiểm tra đăng nhập lấy thông tin thành viên
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if (tv != null)
            {
                // lấy ra list quyền của thành viên tương ứng với loại thành viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //Duyệt list quyền
                string Quyen = "";
                if(lstQuyen.Count() != 0)
                { 
                foreach(var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);//Cắt đi dấu phẩy cuối
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                    Session["TaiKhoan"] = tv;
                    return Content("<script>window.location.reload();</script>");
                }
            }
            return Content("Tài khoản hoặc mật khẩu không đúng");
        }
   public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, TaiKhoan,
                DateTime.Now,//Thời gian khởi tạo
                DateTime.Now.AddHours(3),//Thời gian kết thúc
                false, //Ghi nhớ
                Quyen,
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(cookie);

        }
        //Tạo trang ngăn chặn quyền truy cập
        public ActionResult LoiPhanQuyen()
        {

            return View();
        }
        public ActionResult XemThongTin()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}