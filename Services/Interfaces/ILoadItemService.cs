using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestZL.Models.DTO;
using TestZL.Models.ViewModel;

namespace TestZL.Services.Interfaces
{
    public interface ILoadItemService
    {

        /// <summary>
        /// Сохранить новый элемент
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> SaveNewItemAsync(LoadItemViewModel model);

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<LoadItemTable>> GetAllAsync();


    }
}
