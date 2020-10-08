using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestZL.Services.Interfaces
{
    public interface IDictService
    {
        /// <summary>
        /// Получить справочник вариантов расположения разметки 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectListItem>> GetMarkupsAsync();
    }
}
