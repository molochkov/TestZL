using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using TestZL.DAL;
using TestZL.Services.Interfaces;

namespace TestZL.Services
{
    public class DictService : IDictService
    {
        private readonly TestZLContext context;

        public DictService(
            TestZLContext context
        )
        {
            this.context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetMarkupsAsync()
        {
            List<SelectListItem> markups = await context.LocationMarkups.AsNoTracking()
               .Select(x =>
                   new SelectListItem
                   {
                       Value = x.Id.ToString(),
                       Text = x.Name
                   }).ToListAsync();


            var emptyMarkup = new SelectListItem()
            {
                Value = null,
                Selected = true,
                Text = "--- Выберите расположение разметки ---"
            };
            markups.Insert(0, emptyMarkup);
            return new SelectList(markups, "Value", "Text");
        }
    }
}
