using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult ChuaThanhToan()
        {
            //Lấy danh sách các đơn hàng chưa duyệt
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            //Lấy danh sách đơn hàng chưa giao
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == true).OrderBy(n => n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lstDGDTT = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true);
            return View(lstDGDTT);

        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra đơn hàng tồn tại không
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            //Truy vấn lấy ra dữ liệu của đơn hàng đó'
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            //lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            //Gui khach hang 1 mail de xac nhan viec thanh toan
            GuiEmail("Xác nhận đơn hàng của hệ thống","nguyenthiphuong16031987pv01@gmail.com","chungnguyen060293@gmail.com","01645825680","Đơn hàng của bạn đã đặt thành công");
            return View(ddhUpdate);
        }
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord,string Content)
        {
            //goi Email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); //Dia chi nhan
            mail.From = new MailAddress(ToEmail); //Dia chi gui
            mail.Subject = Title; //Tieu de gui
            mail.Body = Content; // Noi dun
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //host gui cua gmail
            smtp.Port = 587; //post cua Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
                (FromEmail, PassWord); //Tai khoan password nguoi gui
            smtp.EnableSsl = true; //kich hoat giao thiep an toan ssl
            /*smtp.Send(mail);*/ //gui mail di
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