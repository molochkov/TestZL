using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using TestZL.DAL;
using TestZL.Helpers;
using TestZL.Services.Interfaces;

namespace TestZL.Services
{
    public class FileService : IFileService
    {

        private readonly IWebHostEnvironment appEnvironment;
        private readonly ConfigurationHelper сonfigurationHelper;

        public FileService(IWebHostEnvironment appEnvironment, ConfigurationHelper сonfigurationHelper)
        {
            this.appEnvironment = appEnvironment;
            this.сonfigurationHelper = сonfigurationHelper;
         
        }

        /// <summary>
        /// Загрузка файла в локальню папку
        /// </summary>
        /// <param name="SavePath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<String> UploadFileAsync(string SavePath, IFormFile file)
        {
            string path = string.Empty;
            if (file != null)
            {
                CheckCreateFolder(SavePath);
                path = $"{SavePath}{file.FileName}";
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            //если сохранение прошло успешно возвращаем полный путь к файлу
            return path;
        }

        public async Task DeleteDirectoryFileAsync(string FilePath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path.GetDirectoryName(FilePath));
            // Delete this dir and all subdirs.
            try
            {
                di.Delete(true);
            }
            catch (Exception e)
            { 
            }
        }

        public string GetPathToTempFileDirectory(string Tempfolder)
        {
            return this.appEnvironment.WebRootPath + $"/Files/Temp_{Tempfolder}/";
        }
        public string GetPathToFileDirectory(string folder)
        {
            return this.appEnvironment.WebRootPath + $"/Files/{сonfigurationHelper.FolderNameforValidFile()}/{folder}/";
        }

        public async Task CopyFileAsync(string FilePathFrom, string FilePathTo)
        {
            //Создаем директорию назначения, если она отсутствует
            CheckCreateFolder(Path.GetDirectoryName(FilePathTo));
            File.Copy(FilePathFrom, FilePathTo);
        }


        /// <summary>Проверить наличие папки и создать при отсутствии</summary>
        /// <param name="path">Полный путь к папке</param>
        private  void CheckCreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (IOException)
                {
                    throw;
                }
            }
        }

     
    }
}
