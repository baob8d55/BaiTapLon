using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BaiTapLon.Models.DM;

namespace BaiTapLon.Models;

public partial class TbCongTrinhCoSoVatChat
{
    [DisplayName("ID công trình cơ sở vật chất")]
    public int IdCongTrinhCoSoVatChat { get; set; }

    [DisplayName("Mã công trình")]
    [Required(ErrorMessage = "Ô này không được để trống")]
    public string? MaCongTrinh { get; set; }

    [DisplayName("Tên công trình")]
    [Required(ErrorMessage = "Ô này không được để trống")]
    public string? TenCongTrinh { get; set; }

    [DisplayName("Loại công trình")]
    [Required(ErrorMessage = "Ô này không được để trống")]
    public int? IdLoaiCongTrinh { get; set; }

    [DisplayName("Mục đích sử dụng")]
    [Required(ErrorMessage = "Ô này không được để trống")]
    public int? IdMucDichSuDung { get; set; }

    [DisplayName("Đối tượng sử dụng")]
    public string? DoiTuongSuDung { get; set; }

    [DisplayName("Diện tích xây dựng")]
    [Required(ErrorMessage = "Ô này không được để trống")]
    public double? DienTichSanXayDung { get; set; }

    [DisplayName("Vốn ban đầu")]
    public double? VonBanDau { get; set; }

    [DisplayName("Vốn đầu tư")]
    public double? VonDauTu { get; set; }

    [DisplayName("Tình trạng CSVC")]
    public int? IdTinhTrangCsvc { get; set; }

    [DisplayName("Hình thức sở hữu")]
    public int? IdHinhThucSoHuu { get; set; }

    [DisplayName("Công trình CSVC trong nhà")]
    public int? CongTrinhCsvctrongNha { get; set; }

    [DisplayName("Số phòng ở công vụ")]
    public int? SoPhongOcongVu { get; set; }

    [DisplayName("Số chỗ ở cho cán bộ giảng dạy")]
    public int? SoChoOchoCanBoGiangDay { get; set; }

    [DisplayName("Năm đưa vào sử dụng")]
    // Đưa vào định dạng yyyy
    public string? NamDuaVaoSuDung { get; set; }

    [DisplayName("Công trình CSVC trong Nhà")]
    public virtual DmTuyChon? CongTrinhCsvctrongNhaNavigation { get; set; }

    [DisplayName("Hình thức sở hữu")]
    public virtual DmHinhThucSoHuu? IdHinhThucSoHuuNavigation { get; set; }

    [DisplayName("Loại công trình")]
    public virtual DmLoaiCongTrinhCoSoVatChat? IdLoaiCongTrinhNavigation { get; set; }

    [DisplayName("Mục đích sử dụng")]
    public virtual DmMucDichSuDungCsvc? IdMucDichSuDungNavigation { get; set; }

    [DisplayName("Tình trạng CSVC")]
    public virtual DmTinhTrangCoSoVatChat? IdTinhTrangCsvcNavigation { get; set; }

    [DisplayName("Phòng thí nghiệm")]
    public virtual ICollection<TbPhongThiNghiem> TbPhongThiNghiems { get; set; } = new List<TbPhongThiNghiem>();

    [DisplayName("Phòng thực hành")]
    public virtual ICollection<TbPhongThucHanh> TbPhongThucHanhs { get; set; } = new List<TbPhongThucHanh>();

    [DisplayName("Thiết bị PTN-TH trên 500Tr")]
    public virtual ICollection<TbThietBiPtnThtren500Tr> TbThietBiPtnThtren500Trs { get; set; } = new List<TbThietBiPtnThtren500Tr>();
}