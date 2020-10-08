using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestZL.Helpers
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        /// <summary>
        /// Получить минимальное значение файлов в архиве
        /// </summary>     
        public int MinFilesInZip()
        {
            string result = this.configuration["MinFilesInZip"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("MinFilesInZip не найден в  appsettings.");
            }

            int.TryParse(result, out int byteResult);

            return byteResult;
        }

        /// <summary>
        /// Получить Максимальное значение файлов в архиве
        /// </summary>     
        public int MaxFilesInZip()
        {
            string result = this.configuration["MaxFilesInZip"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("MaxFilesInZip не найден в  appsettings.");
            }

            int.TryParse(result, out int byteResult);

            return byteResult;
        }

        /// <summary>
        /// Получить шаг увеличения границ кол-во файлов
        /// </summary>     
        public int UpLimit()
        {
            string result = this.configuration["UpLimit"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("UpLimit не найден в  appsettings.");
            }

            int.TryParse(result, out int byteResult);

            return byteResult;
        }

        /// <summary>
        /// Имя файла с ответами
        /// </summary>
        /// <returns></returns>
        public string AnswerFileName()
        {
            string result = this.configuration["AnswerFileName"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("AnswerFileName не найден в  appsettings.");
            }

            return result;
        }

        /// <summary>
        /// Допустимые графические форматы
        /// </summary>
        /// <returns></returns>
        public string[] AllowImageFormat()
        {
            string result = this.configuration["AllowImageFormat"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("AllowImageFormat не найден в  appsettings.");
            }
            return result.Split(',');
        }

        /// <summary>
        /// Синомим в справочнике - расположение разметки в отдельном файле
        /// </summary>
        /// <returns></returns>
        public string SynAnswerInSeparateFile()
        {
            string result = this.configuration["SynAnswerInSeparateFile"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("SynAnswerInSeparateFile не найден в  appsettings.");
            }

            return result;
        }

        /// <summary>
        /// Папка для валидных архивов
        /// </summary>
        /// <returns></returns>
        public string FolderNameforValidFile()
        {
            string result = this.configuration["FolderNameforValidFile"];

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ArgumentException("FolderNameforValidFile не найден в  appsettings.");
            }

            return result;
        }
    }
}
