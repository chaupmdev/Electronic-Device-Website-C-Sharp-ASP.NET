using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WedSiteBanHang.Models;

namespace WedSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
       
        // GET: QuanLySanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));
        }
        [HttpGet]
        public ActionResult TaoMoi()
           
        {
            //Load dropdowlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Load dropdowlist loai sản phẩm
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            //Load dropdowlist Nhà sản xuất
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        //post và lưu hình ảnh
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp,HttpPostedFileBase[] HinhAnh)
        {
            //Load dropdowlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Load dropdowlist loai sản phẩm
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            //Load dropdowlist Nhà sản xuất
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            //Kiểm tra hình ảnh tồn tại chưa trong csdl
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiểm tra nội dung hình ảnh

                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //Kiểm tra định dạng hình ảnh
                        if (HinhAnh[i].ContentType != "image/jpg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpeg")
                        {
                            ViewBag.upload += "Hình ảnh " + i + " không hợp lệ <br />";
                            loi++;
                        }
                        else
                        {
                            //Lấy tên hình ảnh
                            var fileName = Path.GetFileName(HinhAnh[i].FileName);
                            //Lấy hình ảnh chuyển vào thư mục hình ảnh
                            var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                            //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload1 = "Hình" + i + " đã tồn tại <br />";
                                loi++;

                            }
                            else
                            {
                                HinhAnh[i].SaveAs(path);
                                sp.HinhAnh = fileName;
                            }
                          

                        }

                    }

                }
            }
            if (loi > 0)
            {
                return View(sp);
            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // Hiển thị sản phẩm cần sửa
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            // Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                //Trang dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //Load dropdowlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            //Load dropdowlist loai sản phẩm
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            //Load dropdowlist Nhà sản xuất
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase[] HinhAnh)
        {
            //Load dropdowlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            //Load dropdowlist loai sản phẩm
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            //Load dropdowlist Nhà sản xuất
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            //===================
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiểm tra nội dung hình ảnh

                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //Kiểm tra định dạng hình ảnh
                        if (HinhAnh[i].ContentType != "image/jpg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpeg")
                        {
                            ViewBag.upload += "Hình ảnh " + i + " không hợp lệ <br />";
                            loi++;
                        }
                        else
                        {
                            //Lấy tên hình ảnh
                            var fileName = Path.GetFileName(HinhAnh[i].FileName);
                            //Lấy hình ảnh chuyển vào thư mục hình ảnh
                            var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                            //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                            //if (System.IO.File.Exists(path))
                            //{
                            //    ViewBag.upload1 = "Hình" + i + " đã tồn tại <br />";
                            //    loi++;

                            //}
                            //else
                            //{
                                HinhAnh[i].SaveAs(path);
                                model.HinhAnh = fileName;
                           // }


                        }

                    }

                }
            }
            if (loi > 0)
            {
                return View(model);
            }
            //Nếu dữ liệu đầu vào chắc chắn ok
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();// Lưu sản phẩm sau khi chỉnh sửa
            return RedirectToAction("Index");
           
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            // Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                //Trang dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //Load dropdowlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            //Load dropdowlist loai sản phẩm
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            //Load dropdowlist Nhà sản xuất
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
         
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                if(sp == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(sp);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}