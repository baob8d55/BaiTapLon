using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Models;
using BaiTapLon.API;
using BaiTapLon.Models.DM;
using Newtonsoft.Json;

namespace BaiTapLon.Controllers.CSVC
{

    public class ThuVienTrungTamHocLieuController : Controller
    {
        private Dictionary<string, int> dic = new Dictionary<string, int>();
        private Dictionary<string, int> dic1 = new Dictionary<string, int>();
        private readonly ApiServices ApiServices_;
        private string ds_;

        // Lấy từ HemisContext 
        public ThuVienTrungTamHocLieuController(ApiServices _services)
        {
            ApiServices_ = _services;
        }


        // GET: ThuVienTrungTamHocLieu
        // Lấy danh sách TVTTHL từ database, trả về view Index.

        private async Task<List<TbThuVienTrungTamHocLieu>> TbThuVienTrungTamHocLieus()
            {
                List<TbThuVienTrungTamHocLieu> tbThuVienTrungTamHocLieus = await ApiServices_.GetAll<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu");
                List<DmHinhThucSoHuu> dmhinhThucSoHuus = await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu");
                List<DmTinhTrangCoSoVatChat> dmtinhTrangCoSoVatChats = await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat");
                tbThuVienTrungTamHocLieus.ForEach(item =>
                {
                    item.IdHinhThucSoHuuNavigation = dmhinhThucSoHuus.FirstOrDefault(x => x.IdHinhThucSoHuu == item.IdHinhThucSoHuu);
                    item.IdTinhTrangCsvcNavigation = dmtinhTrangCoSoVatChats.FirstOrDefault(x => x.IdTinhTrangCoSoVatChat == item.IdTinhTrangCsvc);
                });
                return tbThuVienTrungTamHocLieus;
            }

            public async Task<IActionResult> Index()
            {
                try
                {
                    List<TbThuVienTrungTamHocLieu> getall = await TbThuVienTrungTamHocLieus();
                    // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                    return View(getall);
                    // Bắt lỗi các trường hợp ngoại lệ
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }
            public async Task<IActionResult> Chart()
            {
                try
                {
                    List<TbThuVienTrungTamHocLieu> getall = await TbThuVienTrungTamHocLieus();
                    // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                    return View(getall);
                    // Bắt lỗi các trường hợp ngoại lệ
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdThuVienTrungTamHocLieu)
            // Hiển thị bản ghi đó ở view Details
            public async Task<IActionResult> Details(int? id)
            {
                try
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                    var tbThuVienTrungTamHocLieus = await TbThuVienTrungTamHocLieus();
                    var tbThuVienTrungTamHocLieu = tbThuVienTrungTamHocLieus.FirstOrDefault(m => m.IdThuVienTrungTamHocLieu == id);
                    // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                    if (tbThuVienTrungTamHocLieu == null)
                    {
                        return NotFound();
                    }
                    // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                    // Hiển thị thông thi chi tiết TVTTHL thành công
                    return View(tbThuVienTrungTamHocLieu);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            // GET: ThuVienTrungTamHocLieu/Create
            // Hiển thị view Create để tạo một bản ghi TVTTHL mới
            // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
            public async Task<IActionResult> Create()
            {
                try
                {
                    ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu");
                    ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat");
                    return View();
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            // POST: ThuVienTrungTamHocLieu/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

            // Thêm một TVTTHL mới vào Database nếu IdThuVienTrungTamHocLieu truyền vào không trùng với Id đã có trong Database
            // Trong trường hợp nhập trùng IdThuVienTrungTamHocLieu sẽ bắt lỗi
            // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdThuVienTrungTamHocLieu đã có
            [HttpPost]
            [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
            public async Task<IActionResult> Create([Bind("IdThuVienTrungTamHocLieu,MaThuVienTrungTamHocLieu,TenThuVienTrungTamHocLieu,NamDuaVaoSuDung,DienTich,DienTichPhongDoc,SoPhongDoc,SoLuongMayTinh,SoLuongChoNgoi,SoLuongSach,SoLuongTapChi,SoLuongSachDienTu,SoLuongTapChiDienTu,SoLuonngThuVienDienTuLienKetNn,IdTinhTrangCsvc,IdHinhThucSoHuu,SoLuongDauSach,SoLuongDauTapChi,SoLuongDauSachDienTu,SoLuongDauTapChiDienTu")] TbThuVienTrungTamHocLieu tbThuVienTrungTamHocLieu)
            {
                try
                {
                    // Nếu trùng IDThuVienTrungTamHocLieu sẽ báo lỗi
                    if (await TbThuVienTrungTamHocLieuExists(tbThuVienTrungTamHocLieu.IdThuVienTrungTamHocLieu)) ModelState.AddModelError("IdThuVienTrungTamHocLieu", "ID này đã tồn tại!");
                    if (ModelState.IsValid)
                    {
                        await ApiServices_.Create<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", tbThuVienTrungTamHocLieu);
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbThuVienTrungTamHocLieu.IdHinhThucSoHuu);
                    ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbThuVienTrungTamHocLieu.IdTinhTrangCsvc);
                    return View(tbThuVienTrungTamHocLieu);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            // GET: ThuVienTrungTamHocLieu/Edit
            // Lấy data từ Database với Id đã có, sau đó hiển thị ở view Edit
            // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
            // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong database
            public async Task<IActionResult> Edit(int? id)
            {
                //try
                //{
                if (id == null)
                {
                    return NotFound();
                }

                var tbThuVienTrungTamHocLieu = await ApiServices_.GetId<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", id ?? 0);
                if (tbThuVienTrungTamHocLieu == null)
                {
                    return NotFound();
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbThuVienTrungTamHocLieu.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbThuVienTrungTamHocLieu.IdTinhTrangCsvc);
                return View(tbThuVienTrungTamHocLieu);
                //}
                //catch (Exception ex)
                //{
                //    return BadRequest();
                //}

            }

            // POST: ThuVienTrungTamHocLieu/Edit
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

            // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdThuVienTrungTamHocLieu cần chỉnh sửa
            // Nó chỉ cập nhật khi ModelState hợp lệ
            // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

            [HttpPost]
            [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
            public async Task<IActionResult> Edit(int id, [Bind("IdThuVienTrungTamHocLieu,MaThuVienTrungTamHocLieu,TenThuVienTrungTamHocLieu,NamDuaVaoSuDung,DienTich,DienTichPhongDoc,SoPhongDoc,SoLuongMayTinh,SoLuongChoNgoi,SoLuongSach,SoLuongTapChi,SoLuongSachDienTu,SoLuongTapChiDienTu,SoLuonngThuVienDienTuLienKetNn,IdTinhTrangCsvc,IdHinhThucSoHuu,SoLuongDauSach,SoLuongDauTapChi,SoLuongDauSachDienTu,SoLuongDauTapChiDienTu")] TbThuVienTrungTamHocLieu tbThuVienTrungTamHocLieu)
            {
                try
                {
                    if (id != tbThuVienTrungTamHocLieu.IdThuVienTrungTamHocLieu)
                    {
                        return NotFound();
                    }
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            await ApiServices_.Update<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", id, tbThuVienTrungTamHocLieu);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (await TbThuVienTrungTamHocLieuExists(tbThuVienTrungTamHocLieu.IdThuVienTrungTamHocLieu) == false)
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "HinhThucSoHuu", tbThuVienTrungTamHocLieu.IdHinhThucSoHuu);
                    ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbThuVienTrungTamHocLieu.IdTinhTrangCsvc);
                    return View(tbThuVienTrungTamHocLieu);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            // GET: ThuVienTrungTamHocLieu/Delete
            // Xóa một TVTTHL khỏi Database
            // Lấy data TVTTHL từ Database, hiển thị Data tại view Delete
            // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
            public async Task<IActionResult> Delete(int? id)
            {
                try
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                var tbThuVienTrungTamHocLieus = await TbThuVienTrungTamHocLieus();
                var tbThuVienTrungTamHocLieu = tbThuVienTrungTamHocLieus.FirstOrDefault(m => m.IdThuVienTrungTamHocLieu == id);
                if (tbThuVienTrungTamHocLieu == null)
                    {
                        return NotFound();
                    }

                    return View(tbThuVienTrungTamHocLieu);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            // POST: ThuVienTrungTamHocLieu/Delete
            // Xóa TVTTHL khỏi Database sau khi nhấn xác nhận 
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một TVTTHL
            {
                try
                {
                    await ApiServices_.Delete<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }

            private async Task<bool> TbThuVienTrungTamHocLieuExists(int id)
            {
                var tbThuVienTrungTamHocLieus = await ApiServices_.GetAll<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu");
                return tbThuVienTrungTamHocLieus.Any(e => e.IdThuVienTrungTamHocLieu == id);
            }
            public async Task<IActionResult> Receive(string json)
            {
                try
                {
                    // Khai báo thông báo mặc định
                    var message = "No Lỗi";

                    // Giải mã dữ liệu JSON từ client
                    List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                    // Danh sách lưu các đối tượng TbThuVienTrungTamHocLieu
                    List<TbThuVienTrungTamHocLieu> lst = new List<TbThuVienTrungTamHocLieu>();

                    // Khởi tạo Random để tạo ID ngẫu nhiên
                    Random rnd = new Random();

                    // Duyệt qua từng dòng dữ liệu từ Excel
                    foreach (var item in data)
                    {
                        TbThuVienTrungTamHocLieu model = new TbThuVienTrungTamHocLieu();

                        // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                        int id;
                        do
                        {
                            id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                        } while (await TbThuVienTrungTamHocLieuExists(id)); // Kiểm tra id có tồn tại không

                        // Gán dữ liệu cho các thuộc tính của model
                        model.IdThuVienTrungTamHocLieu = id; // Gán ID
                    model.IdHinhThucSoHuu = id; // gán id
                    model.IdTinhTrangCsvc = id;// gán id
                    model.TenThuVienTrungTamHocLieu = item[0]; //gán tên thư viện trung tâm học liệu cho cột đầu tiên
                        model.NamDuaVaoSuDung = item[1];// Gán năm đưa vào sử dụng (chuyển đổi từ string sang int)
                    model.DienTich = ParseInt(item[2]);// Gán diện tích (chuyển đổi từ string sang int)
                    model.DienTichPhongDoc = ParseInt(item[3]);// Gán S phòng đọc (chuyển đổi từ string sang int)
                    model.SoPhongDoc = ParseInt(item[4]);// Gán phòng đọc (chuyển đổi từ string sang int)
                    model.SoLuongMayTinh = ParseInt(item[5]);// Gán số lượng máy tính(chuyển đổi từ string sang int)
                    model.SoLuongChoNgoi = ParseInt(item[6]);// Gán số lượng chỗ ngồi(chuyển đổi từ string sang int)
                    model.SoLuongSach = ParseInt(item[7]);// Gán số luwojgn sáhc (chuyển đổi từ string sang int)
                    model.SoLuongTapChi = ParseInt(item[8]);// Gán số lượng tạp chí (chuyển đổi từ string sang int)
                    model.SoLuongSachDienTu = ParseInt(item[9]);// Gán số lượng sach điện tử(chuyển đổi từ string sang int)
                    model.SoLuongTapChiDienTu = ParseInt(item[10]);// Gán số lượng tạp chí diện tử (chuyển đổi từ string sang int)
                    model.SoLuonngThuVienDienTuLienKetNn = ParseInt(item[11]);// Gán só lượng thư viện điện tử liên kết (chuyển đổi từ string sang int)
                    model.SoLuongDauSach = ParseInt(item[12]);// Gán lường đàu sách (chuyển đổi từ string sang int)
                    model.SoLuongDauTapChi = ParseInt(item[13]);// Gán lượng tạp chí (chuyển đổi từ string sang int)
                    model.SoLuongDauSachDienTu = ParseInt(item[14]);// Gán lượng đầu sachs điện tử(chuyển đổi từ string sang int)
                    model.SoLuongDauTapChiDienTu = ParseInt(item[15]);// Gán lượng tạp chí điện tử (chuyển đổi từ string sang int)



                    // Thêm model vào danh sách
                    lst.Add(model);
                    }

                    // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                    foreach (var item in lst)
                    {
                        await CreateTbThuVienTrungTamHocLieu(item); // Giả sử có phương thức tạo dữ liệu vào DB
                    }
                    return Accepted(Json(new { msg = message }));
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi, trả về thông báo lỗi
                    return BadRequest(Json(new { msg = ex.Message }));
                }
            }

            private async Task CreateTbThuVienTrungTamHocLieu(TbThuVienTrungTamHocLieu item)
            {
                await ApiServices_.Create<TbThuVienTrungTamHocLieu>("/api/csvc/ThuVienTrungTamHocLieu", item);
            }

            private int? ParseInt(string v)
            {
                if (int.TryParse(v, out int result)) // Nếu chuỗi có thể chuyển thành int
                {
                    return result; // Trả về giá trị int
                }
                else
                {
                    return null; // Nếu không thể chuyển thành int, trả về null
                }
            }

        }
}