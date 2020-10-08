using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestZL.Models.ViewModel
{
    public class LoadItemWithOutFile
    {
        public LoadItemWithOutFile()
        {
            TempFolder = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Ниаменование
        /// </summary>        
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        /// <summary>
        /// Содержит кириллицу
        /// </summary>
        [Display(Name = "Содержит кириллицу")]
        public bool IsContainsCyrillic { get; set; }

        /// <summary>
        /// Содержит латиницу
        /// </summary>
        [Display(Name = "Содержит латиницу")]
        public bool IsContainsLatin { get; set; }

        /// <summary>
        /// Содержит цифры
        /// </summary>
        [Display(Name = "Содержит цифры")]
        public bool IsContainsDigits { get; set; }

        /// <summary>
        /// содержит специальные символы
        /// </summary>
        [Display(Name = "Содержит специальные символы")]
        public bool IsContainsSpecialChars { get; set; }

        /// <summary>
        /// Чувствительность к регистру
        /// </summary>
        [Display(Name = "Чувствительность к регистру")]
        public bool IsCaseSensitivity { get; set; }

        /// <summary>
        /// Расположение разметки
        /// </summary>
        [Display(Name = "Расположение разметки")]
        public string SelectedMarkupId { get; set; }


        /// <summary>
        /// Временная папка сохранения файла
        /// </summary>
        public string TempFolder { get; set; }

    }
}
