using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestZL.Helpers;
using TestZL.Models;

using TestZL.Models.ViewModel;
using TestZL.Services.Interfaces;

namespace TestZL.Controllers
{
    public class HomeController : Controller
    {



        private readonly IFileService fileService;
        private readonly ILoadItemService loadItemService;

        public HomeController(IFileService fileService, ILoadItemService loadItemService)
        {
            this.fileService = fileService;
            this.loadItemService = loadItemService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Load([FromForm]LoadItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //если валидация не прошла, удаляем загруженный во временную папку файл
                await fileService.DeleteDirectoryFileAsync(fileService.GetPathToTempFileDirectory(model.item.TempFolder));
                return View("Index", model);
            }

            // сохраняем данные в базу
            bool resultSave = await this.loadItemService.SaveNewItemAsync(model);
            return RedirectToAction("Index");
        }
    }
}
