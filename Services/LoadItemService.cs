using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestZL.DAL;
using TestZL.Helpers;
using TestZL.Models.Database;
using TestZL.Models.DTO;
using TestZL.Models.ViewModel;
using TestZL.Services.Interfaces;

namespace TestZL.Services
{
    public class LoadItemService : ILoadItemService
    {


        private readonly TestZLContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly ConfigurationHelper сonfigurationHelper;

        public LoadItemService(
          TestZLContext context, IMapper mapper, IFileService fileService, ConfigurationHelper сonfigurationHelper
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.fileService = fileService;
            this.сonfigurationHelper = сonfigurationHelper;
        }

        public async Task<IEnumerable<LoadItemTable>> GetAllAsync()
        {
            return this.mapper.Map<IEnumerable<LoadItemTable>>(await context.LoadItems.ToListAsync());
        }

        public async Task<bool> SaveNewItemAsync(LoadItemViewModel model)
        {

            // преобразуем входящую модель в модель базы данных
            var result = this.mapper.Map<LoadItem>(model);
            result.DateCreation = DateTime.Now;
            result.Markup = context.LocationMarkups.Where(x => x.Id == new Guid(model.item.SelectedMarkupId)).FirstOrDefault();
            context.LoadItems.Add(result);
            context.SaveChanges();


            //копируем файл для постоянного хранения
            await fileService.CopyFileAsync(
                $"{fileService.GetPathToTempFileDirectory(model.item.TempFolder)}{result.FileName}",
                $"{fileService.GetPathToFileDirectory(result.Id.ToString())}{result.FileName}");

            //удаляем временную папку с файлом
            await fileService.DeleteDirectoryFileAsync(fileService.GetPathToTempFileDirectory(model.item.TempFolder));

            result.LocalPathToFile = Path.GetFullPath(fileService.GetPathToFileDirectory(result.Id.ToString()));
            context.LoadItems.Update(result);
            context.SaveChanges();
            return true;
        }       
    }
}
