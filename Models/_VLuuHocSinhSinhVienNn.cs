﻿using System;
using System.Collections.Generic;

namespace BaiTapLon.Models;

public partial class _VLuuHocSinhSinhVienNn
{
    public int id { get; set; }

    public int? IdNguoiHoc { get; set; }

    public string? NguonKinhPhiChoLuuHocSinh { get; set; }

    public string? LoaiLuuHocSinh { get; set; }
}
