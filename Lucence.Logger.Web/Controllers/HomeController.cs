using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lucence.Logger.Web.Models;
using System.IO;
using Framework;
using System.Text;

namespace Lucence.Logger.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            Storage();
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="level"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Logger(String project, String dateTime,String search)
        {
            if (String.IsNullOrWhiteSpace(search)) search = "Warn Info Error Trace";
            if (!DateTime.TryParse(dateTime, out DateTime dt)) return new JsonResult("错误");
            return new JsonResult(LucenceHelper.SearchData(project, search, dt));
        }
        public void Storage()
        {
            String path = Path.Combine(LoggerModel.Path, "testtxt", "write.lock");
            if (System.IO.File.Exists(path))
            {
                return;
            }
            path = Path.Combine(Environment.CurrentDirectory, "testtxt.txt");
            if (System.IO.File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                //文件内容
                using (var contents = new StreamReader(file.FullName, Encoding.UTF8))
                {
                    Random rnd = new Random();
                    while (!contents.EndOfStream)
                    {
                        int level = rnd.Next(0, 4);
                        SealedLogModel detail = new SealedLogModel()
                        {
                            Level = (SealedLogLevel)level,
                            ProjectName = "testtxt",
                            Sign = "测试",
                            Time = DateTime.Now.AddMinutes(level),
                            Value = contents.ReadLine()
                        };
                        LucenceHelper.StorageData(detail);
                    }
                }
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
