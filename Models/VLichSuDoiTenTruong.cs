﻿using System;
using System.Collections.Generic;

namespace BaiTapLon.Models;

public partial class VLichSuDoiTenTruong
{
    public string? TenTruongCu { get; set; }

    public string? TenTruongCuTiengAnh { get; set; }

    public string? SoQuyetDinhDoiTen { get; set; }

    public DateOnly? NgayKyQuyetDinhDoiTen { get; set; }
}
