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
using System.Security.Cryptography;

namespace BaiTapLon.Controllers.CSVC
{


    public class KiTucXaController : Controller
    {
         private Dictionary<string, int> dic = new Dictionary<string, int>();
        private Dictionary<string, int> dic1 = new Dictionary<string, int>();
        private readonly ApiServices ApiServices_;
        private string ds_;

        // Lấy từ HemisContext 
        public KiTucXaController(ApiServices _services)
        {
            ApiServices_ = _services;
        }

        // GET: KiTucXa
        // Lấy danh sách KTX từ database, trả về view Index.
        [HttpPost]
        private async Task<List<TbKiTucXa>> TbKiTucXas()
        {
            List<TbKiTucXa> tbKiTucXas = await ApiServices_.GetAll<TbKiTucXa>("/api/csvc/KiTucXa");
            List<DmHinhThucSoHuu> dmhinhThucSoHuus = await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu");
            List<DmTinhTrangCoSoVatChat> dmtinhTrangCoSoVatChats = await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat");
            tbKiTucXas.ForEach(item => {
                item.IdHinhThucSoHuuNavigation = dmhinhThucSoHuus.FirstOrDefault(x => x.IdHinhThucSoHuu == item.IdHinhThucSoHuu);
                item.IdTinhTrangCsvcNavigation = dmtinhTrangCoSoVatChats.FirstOrDefault(x => x.IdTinhTrangCoSoVatChat == item.IdTinhTrangCsvc);
            });
            return tbKiTucXas;
        }

        public async Task<IActionResult> Index(string ds)
        {
            try
            {
                List<TbKiTucXa> getall = await TbKiTucXas();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            bool[] check = new bool[100000];
            List<TbKiTucXa>? ds1 = JsonConvert.DeserializeObject<List<TbKiTucXa>>(ds_);
            return Content(JsonConvert.SerializeObject(ds1));
            return View();

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdKiTucXa)
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
                var tbKiTucXas = await TbKiTucXas();
                var tbKiTucXa = tbKiTucXas.FirstOrDefault(m => m.IdKiTucXa == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbKiTucXa == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết KTX thành công
                return View(tbKiTucXa);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: KiTucXa/Create
        // Hiển thị view Create để tạo một bản ghi KTX mới
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

        // POST: KiTucXa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một KTX mới vào Database nếu IdKiTucXa truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdKiTucXa sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdKiTucXa đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdKiTucXa,MaKyTucxa,IdHinhThucSoHuu,TongChoO,TongDienTich,IdTinhTrangCsvc,SoPhong,NamDuaVaoSuDung")] TbKiTucXa tbKiTucXa)
        {
            try
            {
                // Nếu trùng IDKiTucXa sẽ báo lỗi
                if (await TbKiTucXaExists(tbKiTucXa.IdKiTucXa)) ModelState.AddModelError("IdKiTucXa", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbKiTucXa>("/api/csvc/KiTucXa", tbKiTucXa);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/DmHinhThucSoHuu"), "IdHinhThucSoHuu", "IdHinhThucSoHuu", tbKiTucXa.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbKiTucXa.IdTinhTrangCsvc);
                return View(tbKiTucXa);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: KiTucXa/Edit
        // Lấy data từ Database với Id đã có, sau đó hiển thị ở view Edit
        // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
        // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong database
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbKiTucXa = await ApiServices_.GetId<TbKiTucXa>("/api/csvc/KiTucXa", id ?? 0);
                if (tbKiTucXa == null)
                {
                    return NotFound();
                }
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "IdHinhThucSoHuu", tbKiTucXa.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbKiTucXa.IdTinhTrangCsvc);
                return View(tbKiTucXa);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: KiTucXa/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdKiTucXa cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdKiTucXa,MaKyTucxa,IdHinhThucSoHuu,TongChoO,TongDienTich,IdTinhTrangCsvc,SoPhong,NamDuaVaoSuDung")] TbKiTucXa tbKiTucXa)
        {
            try
            {
                if (id != tbKiTucXa.IdKiTucXa)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbKiTucXa>("/api/csvc/KiTucXa", id, tbKiTucXa);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbKiTucXaExists(tbKiTucXa.IdKiTucXa) == false)
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
                ViewData["IdHinhThucSoHuu"] = new SelectList(await ApiServices_.GetAll<DmHinhThucSoHuu>("/api/dm/HinhThucSoHuu"), "IdHinhThucSoHuu", "IdHinhThucSoHuu", tbKiTucXa.IdHinhThucSoHuu);
                ViewData["IdTinhTrangCsvc"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangCoSoVatChat>("/api/dm/TinhTrangCoSoVatChat"), "IdTinhTrangCoSoVatChat", "TinhTrangCoSoVatChat", tbKiTucXa.IdTinhTrangCsvc);
                return View(tbKiTucXa);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: KiTucXa/Delete
        // Xóa một KTX khỏi Database
        // Lấy data KTX từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbKiTucXas = await ApiServices_.GetAll<TbKiTucXa>("/api/csvc/KiTucXa");
                var tbKiTucXa = tbKiTucXas.FirstOrDefault(m => m.IdKiTucXa == id);
                if (tbKiTucXa == null)
                {
                    return NotFound();
                }

                return View(tbKiTucXa);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: KiTucXa/Delete
        // Xóa KTX khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một KTX
        {
            try
            {
                await ApiServices_.Delete<TbKiTucXa>("/api/csvc/KiTucXa", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbKiTucXaExists(int id)
        {
            var tbKiTucXas = await ApiServices_.GetAll<TbKiTucXa>("/api/csvc/KiTucXa");
            return tbKiTucXas.Any(e => e.IdKiTucXa == id);
        }
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "No Lỗi";

                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbKiTucXa
                List<TbKiTucXa> lst = new List<TbKiTucXa>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();

                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbKiTucXa model = new TbKiTucXa();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (await TbKiTucXaExists(id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdKiTucXa = id; // Gán ID
                    model.MaKyTucxa = item[0]; // Gán mã ký túc xá từ cột đầu tiên
                    model.TongChoO = ParseInt(item[1]); // Gán tổng chỗ ở (chuyển đổi từ string sang int)
                    model.TongDienTich = ParseInt(item[2]); // Gán tổng diện tích (chuyển đổi từ string sang int)
                    model.SoPhong = ParseInt(item[3]); // Gán số phòng (chuyển đổi từ string sang int)
                    model.IdHinhThucSoHuu = ParseInt(item[4]); // Gán hình thức sở hữu (chuyển đổi từ string sang int)
                    model.IdTinhTrangCsvc = ParseInt(item[5]); // Gán tình trạng CSVC (chuyển đổi từ string sang int)

                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbKiTucXa(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbKiTucXa(TbKiTucXa item)
        {
            await ApiServices_.Create<TbKiTucXa>("/api/csvc/KiTucXa", item);
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