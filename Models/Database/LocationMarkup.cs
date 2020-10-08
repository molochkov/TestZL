using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestZL.Models.Database
{
    public class LocationMarkup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование расположения разметки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Синоним
        /// </summary>
        public string Syn { get; set; }

    }
}
