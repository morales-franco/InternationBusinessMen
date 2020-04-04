using AutoMapper;

namespace InternationalBusinessMen.Api.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Core.Entities.Rate, Dtos.RateDto>()
                .ForMember(d => d.Rate, opt => opt.MapFrom(s => s.RateValue));

            CreateMap<Core.Entities.Transaction, Dtos.TransactionDto>();
        }
    }
}
