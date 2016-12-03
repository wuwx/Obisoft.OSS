using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Obisoft_OSS.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Index(string id)
        {
            var file = Request.Form.Files["file"];
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (file != null && file.Length < 8192000)
            {
                var _wwwroot = Directory.GetCurrentDirectory() + @"\wwwroot";
                var _filename = Path.GetFileName(file.FileName);
                var _path = _wwwroot + @"\" + _filename;
                var _fileStream = new FileStream(path: _path, mode: FileMode.Create);
                await file.CopyToAsync(_fileStream);
                _fileStream.Dispose();
                var FileWebName = Request.Scheme + "://" + Request.Host.ToString() + Request.Path+_filename;
                return Json(new { path = FileWebName, code = 0 });
            }
            return Json(new { code = -1 });
        }
    }
}
