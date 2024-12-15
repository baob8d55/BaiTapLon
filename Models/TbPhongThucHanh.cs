using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BaiTapLon.Models.DM;

namespace BaiTapLon.Models;

public partial class TbPhongThucHanh
{
    [DisplayName("Id Phòng thực hành ")]
    [Range(1, int.MaxValue, ErrorMessage = "Id phải là số dương.")]
    public int IdPhongThucHanh { get; set; }

    [DisplayName("Id Công trình")]
    public int? IdCongTrinhCsvc { get; set; }

    [DisplayName("Id Lĩnh vực")]
    public int? IdLinhVuc { get; set; }

    [DisplayName("Mức độ đáp ứng nhu cầu NCKH")]
    public string? MucDoDapUngNhuCauNckh { get; set; }

    [DisplayName("Năm đưa vào sử dụng")]
    public string? NamDuaVaoSuDung { get; set; }

    [DisplayName("Công trình CSVC")]
    public virtual TbCongTrinhCoSoVatChat? IdCongTrinhCsvcNavigation { get; set; }

    [DisplayName("Lĩnh vực")]
    public virtual DmLinhVucNghienCuu? IdLinhVucNavigation { get; set; }
}