using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Obisoft.OSS.Services;
using Obisoft.OSS.Models;
using Newtonsoft.Json;
using System.Net;

namespace Obisoft.OSS.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Index(string token)
        {
            try
            {
                HTTPService HTTP = new HTTPService();
                HTTP.cc.Add(new Uri("https://www.obisoft.com.cn"), new Cookie("Token", token));
                var Response = await HTTP.Get($"https://www.obisoft.com.cn/api/validatetoken");
                var Result = JsonConvert.DeserializeObject<ValidateToken>(Response);
                if (Result.Code == 0)
                {
                    var file = Request.Form.Files[0];
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (file != null && file.Length < 8192000)
                    {
                        var _filename = Path.GetFileName(file.FileName).Replace(" ", "_");
                        var _wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var _path = Path.Combine(_wwwroot, _filename);
                        var _fileStream = new FileStream(path: _path, mode: FileMode.Create);
                        await file.CopyToAsync(_fileStream);
                        _fileStream.Dispose();
                        var FileWebName = Request.Scheme + "://" + Request.Host.ToString() + Request.Path + _filename;
                        return Json(new { path = FileWebName, code = 0 });
                    }
                    return Json(new { Result = "Your file is larger than 819200!", code = -1 });
                }
                else
                {
                    return Json(new { Result = "Error with your appid or appsecret", Code = -10 });
                }
            }
            catch
            {
                return Json(new { code = -5 });
            }
        }
    }
}
