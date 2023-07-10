using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    public class PhanQuyenController : Controller
    {
        // GET: PhanQuyen
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.LoaiThanhViens.OrderBy(n => n.TenLoai));
        }
        [HttpGet]
        public ActionResult PhanQuyen(int? id)
        {
            //Lấy mã loại tv dựa vào id
            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if(ltv == null)
            {
                return HttpNotFound();
            }
            //Lấy ra danh sách quyền
            ViewBag.MaQuyen = db.Quyens;
            //Lấy ra danh sách quyền của loại thành viên đó
            //Bước 1: lấy ra quyền thuộc loại thành viên đó dựa vào bảng loại thành viên
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);
            return View(ltv);
        }
        [HttpPost]
        public ActionResult PhanQuyen(int? MaLTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {
            //Trường hợp: Nếu đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //Bước 1: Xóa những quyền của thuộc loại TV đó
            var lstDaPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV);
            if(lstDaPhanQuyen.Count() != 0)
            {
                db.LoaiThanhVien_Quyen.RemoveRange(lstDaPhanQuyen);
                db.SaveChanges();

            }
            if(lstPhanQuyen != null)
            {
                //Kiểm tra list danh sách quyền được check
                foreach (var item in lstPhanQuyen)
                {
                    item.MaLoaiTV = int.Parse(MaLTV.ToString());
                    //Nếu được check thì insert dữ liệu vào bảng phân quyền
                    db.LoaiThanhVien_Quyen.Add(item);

                }
                db.SaveChanges();
            }
            
            
            return RedirectToAction("Index");
        }
    }
}