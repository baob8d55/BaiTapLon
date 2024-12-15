﻿using BaiTapLon.Models.DM;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models;

public partial class TbPhongThiNghiem
{
    [DisplayName("Id Phòng thí nghiệm")]
    [Range(1, int.MaxValue, ErrorMessage = "Id phải là số dương.")]
    public int IdPhongThiNghiem { get; set; }
    
    [DisplayName("Id Công trình")]
    public int? IdCongTrinhCsvc { get; set; }

    [DisplayName("Loại phòng thí nghiệm")]
    public int? IdLoaiPhongThiNghiem { get; set; }

    [DisplayName("Lĩnh vực")]
    public int? IdLinhVuc { get; set; }

    [DisplayName("Mức độ đáp ứng nhu cầu NCKH")]
    public string? MucDoDapUngNhuCauNckh { get; set; }

    [DisplayName("Năm đưa vào sử dụng")]
    public string? NamDuaVaoSuDung { get; set; } 
    
    [DisplayName("Công trình cơ sở vật chất")]
    public virtual TbCongTrinhCoSoVatChat? IdCongTrinhCsvcNavigation { get; set; }

    [DisplayName("Lĩnh vực")]
    public virtual DmLinhVucNghienCuu? IdLinhVucNavigation { get; set; }

    [DisplayName("Loại phòng thí nghiệm")]
    public virtual DmLoaiPhongThiNghiem? IdLoaiPhongThiNghiemNavigation { get; set; }
}