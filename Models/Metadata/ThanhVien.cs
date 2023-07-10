using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WedSiteBanHang.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            [DisplayName("Mã thành viên")]
            public int MaThanhVien { get; set; }
            [DisplayName("Tài khoản")]
            [Required(ErrorMessage ="{0} Tài khoản không được trống!")]
            public string TaiKhoan { get; set; }
            public string MatKhau { get; set; }
            public string HoTen { get; set; }
            public string DiaChi { get; set; }
            public string Email { get; set; }
            public string SoDienThoai { get; set; }
            public string CauHoi { get; set; }
            public string CauTraLoi { get; set; }
            public Nullable<int> MaLoaiTV { get; set; }

        }
    }
}