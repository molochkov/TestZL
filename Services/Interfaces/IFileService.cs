using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TestZL.Services.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Загрузка файла в локальню папку
        /// </summary>
        /// <param name="SavePath">путь сохранения</param>
        /// <param name="file">объект IFormFile</param>
        /// <returns>полный путь к сохраненому файлу</returns>
        Task<String> UploadFileAsync(string SavePath, IFormFile file);

        /// <summary>
        /// Удаление директории с временным файлом
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        Task DeleteDirectoryFileAsync(string FilePath);

        /// <summary>
        /// копирует файл
        /// </summary>
        /// <param name="FilePathFrom"></param>
        /// <param name="FilePathTo"></param>
        /// <returns></returns>
        Task CopyFileAsync(string FilePathFrom, string FilePathTo);

        /// <summary>
        /// Возвращает полный путь к папке где будет сохранятся временный файл
        /// </summary>
        /// <param name="Tempfolder"></param>
        /// <returns></returns>
        string GetPathToTempFileDirectory(string Tempfolder);
        
        /// <summary>
        /// Возвращает полный путь к папке где будет хранится файл
        /// </summary>
        /// <param name="Tempfolder"></param>
        /// <returns></returns>
        string GetPathToFileDirectory(string Tempfolder);


    }
}
