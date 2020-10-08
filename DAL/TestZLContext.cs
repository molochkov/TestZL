using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestZL.Models.Database;

namespace TestZL.DAL
{
    public class TestZLContext : DbContext
    {

        /// <summary>
        /// Справочник Расположение разметки
        /// </summary>
        public DbSet<LocationMarkup> LocationMarkups { get; set; }
        /// <summary>
        /// Таблица элементов загрузки
        /// </summary>
        public DbSet<LoadItem> LoadItems { get; set; }


        public TestZLContext(DbContextOptions<TestZLContext> options)
            : base(options)
        {         
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=testZL;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            InitData(modelBuilder);
        }

        private void InitData(ModelBuilder builder)
        {
            builder.Entity<LocationMarkup>().HasData(new LocationMarkup[]
           {
                new LocationMarkup()
                {     
                    Id=Guid.NewGuid(),
                    Name = "отсутствует",
                    Syn="None"
                },
                new LocationMarkup()
                {
                    Id=Guid.NewGuid(),
                    Name = "в именах файлов",
                    Syn="InFileName"
                },
                new LocationMarkup()
                {
                    Id=Guid.NewGuid(),
                    Name = "в отдельном файле",
                    Syn="InSeparateFile"
                }
           });

        }
    }

}
