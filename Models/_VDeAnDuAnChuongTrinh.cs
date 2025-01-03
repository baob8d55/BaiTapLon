﻿using System;
using System.Collections.Generic;

namespace BaiTapLon.Models;

public partial class _VDeAnDuAnChuongTrinh
{
    public int id { get; set; } 

    public string? MaDeAnDuAnChuongTrinh { get; set; }

    public string? TenDeAnDuAnChuongTrinh { get; set; }

    public string? NoiDungTomTat { get; set; }

    public string? MucTieu { get; set; }

    public DateOnly? ThoiGianHopTacTu { get; set; }

    public DateOnly? ThoiGianHopTacDen { get; set; }

    public double? TongKinhPhi { get; set; }

    public string? NguonKinhPhiChoDeAn { get; set; }
}
