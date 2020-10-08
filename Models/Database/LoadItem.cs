using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestZL.Models.Database
{
    public class LoadItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Содержит кириллицу
        /// </summary>
        public bool IsContainsCyrillic { get; set; }

        /// <summary>
        /// Содержит латиницу
        /// </summary>
        public bool IsContainsLatin { get; set; }

        /// <summary>
        /// Содержит цифры
        /// </summary>
        public bool IsContainsDigits { get; set; }


        /// <summary>
        /// содержит специальные символы
        /// </summary>
        public bool IsContainsSpecialChars { get; set; }

        /// <summary>
        /// Чувствительность к регистру
        /// </summary>
        public bool IsCaseSensitivity { get; set; }

        /// <summary>
        /// Расположение разметки
        /// </summary>
        public LocationMarkup Markup { get; set; }

        /// <summary>
        /// локальный путь к файлу
        /// </summary>
        public string LocalPathToFile { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }










    }
}
