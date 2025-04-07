using AutoMapper;
using UserIdentityP19.Models;

namespace UserIdentityP19.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Student, StudentViewModel>().ReverseMap();
            CreateMap<StudentViewModel, Student>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<UserViewModel, User>().ReverseMap();
        }
    }
}
