﻿using System;
using System.Collections.Generic;

namespace BaiTapLon.Models;

public partial class _VToChucHopTacQuocTe
{
    public int id { get; set; }

    public string? MaHopTac { get; set; }

    public string? TenToChuc { get; set; }

    public string? TenNuoc { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly? ThoiGianKyKet { get; set; }

    public string? KetQuaHopTac { get; set; }

    public string? LoaiToChuc { get; set; }
}
