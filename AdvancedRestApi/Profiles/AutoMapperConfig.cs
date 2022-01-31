using AdvancedRestApi.DTO_s;
using AdvancedRestApi.Models;
using AutoMapper;

namespace AdvancedRestApi.Profiles
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            //CreateMap<UserDTO, uSER>();  INSTEAD OF DECLARING TWO TIMES
        }
    }
}
