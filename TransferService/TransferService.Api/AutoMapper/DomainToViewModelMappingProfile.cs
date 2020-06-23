using AutoMapper;
using TransferService.Api.ViewModels;
using TransferService.Domain;
using TransferService.Helper.Extensions;

namespace TransferService.Api.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile() : this("Profile")
        {
        }

        protected DomainToViewModelMappingProfile(string profileName) : base(profileName)
        {

            #region [ User ]

            CreateMap<User, UserVM>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            #endregion [ User ]


        }
    }
}