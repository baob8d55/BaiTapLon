﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models;

public partial class VCoCauToChuc2
{
    public int id { get; set; }

    public string? MaPhongBanDonVi { get; set; }

    public string? LoaiPhongBan { get; set; }

    public string? MaPhongBanDonViCha { get; set; }

    public string? TenPhongBanDonVi { get; set; }

    public string? SoQuyetDinhThanhLap { get; set; }

    public string? NgayRaQuyetDinh { get; set; }

    public string? TrangThaiCoSoGd { get; set; }
}
