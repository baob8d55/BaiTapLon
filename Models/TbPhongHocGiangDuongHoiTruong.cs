using System;
using System.Collections.Generic;
using System.ComponentModel;
using BaiTapLon.Models.DM;

namespace BaiTapLon.Models;

public partial class TbPhongHocGiangDuongHoiTruong
{
    [DisplayName("Id Phòng học giảng đường hội trường")]
    public int IdPhongHocGiangDuongHoiTruong { get; set; }

    [DisplayName("Mã số")]
    public string? MaPhongHocGiangDuongHoiTruong { get; set; }

    [DisplayName("Tên Phòng học")]
    public string? TenPhongHocGiangDuongHoiTruong { get; set; }

    [DisplayName("Diện tích")]
    public double? DienTich { get; set; }

    [DisplayName("Hình thức sở hữu")]
    public int? IdHinhThucSoHuu { get; set; }

    [DisplayName("Quy mô chỗ ngồi")]
    public int? QuyMoChoNgoi { get; set; }

    [DisplayName("Tình trạng CSVC")]
    public int? IdTinhTrangCoSoVatChat { get; set; }

    [DisplayName("Phân Loại CSVC")]
    public int? IdPhanLoaiCsvc { get; set; }

    [DisplayName("Loại Phòng học")]
    public int? IdLoaiPhongHoc { get; set; }

    [DisplayName("Loại Đề án")]
    public int? IdLoaiDeAn { get; set; }

    [DisplayName("Năm đưa vào sử dụng")]
    public string? NamDuaVaoSuDung { get; set; }

    [DisplayName("Hình thức sở hữu")]
    public virtual DmHinhThucSoHuu? IdHinhThucSoHuuNavigation { get; set; }

    [DisplayName("Loại đề án")]
    public virtual DmLoaiDeAnChuongTrinh? IdLoaiDeAnNavigation { get; set; }
    
    [DisplayName("Loại phòng học")]
    public virtual DmLoaiPhongHoc? IdLoaiPhongHocNavigation { get; set; }

    [DisplayName("Phân loại CSVC")]
    public virtual DmPhanLoaiCsvc? IdPhanLoaiCsvcNavigation { get; set; }

    [DisplayName("Tình trạng CSVC ")]
    public virtual DmTinhTrangCoSoVatChat? IdTinhTrangCoSoVatChatNavigation { get; set; }
}
