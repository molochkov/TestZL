using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestZL.Models.DTO
{
    public class LoadItemTable
    {
       
        /// <summary>
        /// Наименование
        /// </summary>
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
         [Display(Name = "Дата создания")]
        public DateTime DateCreation { get; set; }
    }
}
