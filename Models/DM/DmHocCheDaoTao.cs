using System;
using System.Collections.Generic;

namespace BaiTapLon.Models.DM;

public partial class DmHocCheDaoTao
{
    public int IdHocCheDaoTao { get; set; }

    public string? HocCheDaoTao { get; set; }

    public virtual ICollection<TbChuongTrinhDaoTao> TbChuongTrinhDaoTaos { get; set; } = new List<TbChuongTrinhDaoTao>();
}
