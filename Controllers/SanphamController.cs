using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;
using PagedList;
using System.Net;

namespace WedSiteBanHang.Controllers
{
    public class SanphamController : Controller
    {

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Tao w partial view phan 1 va 2 de hien thi san pham theo 2 style khac nhau
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
      
       
        // Xây dựng trang xem chi tiết
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            //Kiểm tra tham số truyền vào có rỗng hay không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                //Thông báo nếu như không có sản phẩm
                return HttpNotFound();
            }
            return View(sp);
        }
        // Xây dựng action load sản phẩm theo mã sản phẩm và mã nhà sản xuất
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {
            if(MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
                if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int PageSize = 6;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber, PageSize));
        }
        public ActionResult XemGioiThieu()
        {
            return View();
        }
    }
}