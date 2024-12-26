using System;
using System.Collections.Generic;
using System.ComponentModel;
using BaiTapLon.Models.DM;

namespace BaiTapLon.Models;

public partial class TbThietBiPtnThtren500Tr
{
    public int IdThietBiPtnTh { get; set; }

    [DisplayName("Mã thiết bị")]
    public string? MaThietBi { get; set; }

    [DisplayName("Mã công trình CSVC")]
    public int? IdCongTrinhCsvc { get; set; }

    [DisplayName("Tên thiết bị")]
    public string? TenThietBi { get; set; }

    [DisplayName("Năm sản xuất")]
    public string? NamSanXuat { get; set; }

    [DisplayName("Xuất xứ")]
    public int? IdQuocGiaXuatXu { get; set; }

    [DisplayName("Hãng sản xuất")]
    public string? HangSanXuat { get; set; }

    [DisplayName("Số lượng thiết bị cùng loại")]
    public int? SoLuongThietBiCungLoai { get; set; }

    [DisplayName("Năm đưa vào sử dụng")]
    public string? NamDuaVaoSuDung { get; set; }

    [DisplayName("Công trình cơ sở vật chất")]
    public virtual TbCongTrinhCoSoVatChat? IdCongTrinhCsvcNavigation { get; set; }

    [DisplayName("Quốc gia xuất xứ")]
    public virtual DmQuocTich? IdQuocGiaXuatXuNavigation { get; set; }

}