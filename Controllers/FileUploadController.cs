using Microsoft.AspNetCore.Mvc;
namespace ControllerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController: ControllerBase
{
    // กำหนดตัวแปรเก็บที่เก็บไฟล์อัพโหลด
    private readonly string _uploadFolder;

    public FileUploadController()
    {
        // กำหนดที่เก็บไฟล์อัพโหลด
        _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        // ถ้าไม่มีโฟลเดอร์ uploads ให้สร้างโฟลเดอร์
        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder); // สร้างโฟลเดอร์ uploads
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        // ถ้าไม่มีไฟล์อัพโหลด
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // กำหนดชื่อไฟล์ที่จะเก็บ
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        // กำหนดที่เก็บไฟล์ที่จะเก็บ
        var filePath = Path.Combine(_uploadFolder, fileName);

        // อัพโหลดไฟล์
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // ส่งชื่อไฟล์กลับไป
        return Ok(fileName);
    }
}