using AutoMapper;
using Mvc1VaccinDemo.ViewModels;
using SharedThings.Data;

namespace Mvc1VaccinDemo.Infrastructure.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonEditViewModel>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ReverseMap();
        }
    }
}