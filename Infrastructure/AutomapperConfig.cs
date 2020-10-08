using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestZL.Models.Database;
using TestZL.Models.DTO;
using TestZL.Models.ViewModel;

namespace TestZL.Infrastructure
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            this.CreateMap<LoadItemViewModel, LoadItem>()
              .ForMember(x => x.Name, x => x.MapFrom(y => y.item.Name))
              .ForMember(x => x.IsContainsCyrillic, x => x.MapFrom(y => y.item.IsContainsCyrillic))
              .ForMember(x => x.IsContainsLatin, x => x.MapFrom(y => y.item.IsContainsLatin))
              .ForMember(x => x.IsContainsDigits, x => x.MapFrom(y => y.item.IsContainsDigits))
               .ForMember(x => x.IsContainsSpecialChars, x => x.MapFrom(y => y.item.IsContainsSpecialChars))
               .ForMember(x => x.IsCaseSensitivity, x => x.MapFrom(y => y.item.IsCaseSensitivity))
               .ForMember(x => x.Markup, x => x.MapFrom(y => new LocationMarkup() { Id = new Guid(y.item.SelectedMarkupId) }))
               .ForMember(x => x.FileName, x => x.MapFrom(y => y.uploadedFile.FileName));


            this.CreateMap<LoadItem, LoadItemTable>();

        }
    }
}

