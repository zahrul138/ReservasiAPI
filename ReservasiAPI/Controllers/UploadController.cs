using Microsoft.AspNetCore.Mvc;

namespace ReservasiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Upload multiple images. (form-data: images)
        /// </summary>
        /// <param name="images"></param>
        /// <returns>Urls of uploaded images</returns>
        [HttpPost("multi")]
        [Produces("application/json")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> images)
        {
            try
            {
                if (images == null || images.Count == 0)
                    return BadRequest(new { error = "No files uploaded." });

                var uploadsDir = Path.Combine(_env.WebRootPath ?? "", "uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                var urls = new List<string>();

                foreach (var image in images)
                {
                    // CEK file null/kosong atau gak punya nama
                    if (image == null || image.Length == 0 || string.IsNullOrEmpty(image.FileName))
                        continue;

                    var ext = Path.GetExtension(image.FileName);
                    var fileName = Guid.NewGuid() + ext;
                    var filePath = Path.Combine(uploadsDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    urls.Add($"/uploads/{fileName}");
                }

                if (urls.Count == 0)
                    return BadRequest(new { error = "No valid files uploaded." });

                return Ok(new { urls });
            }
            catch (Exception ex)
            {
                // biar errornya jelas waktu develop/debug
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
