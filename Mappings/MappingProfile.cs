using AutoMapper;
using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Reservation;

namespace DeskBooking.Api.Mappings;

/// <summary>
/// AutoMapper профиль. Используем только там, где реально удобно.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(d => d.DeskNumber, opt => opt.MapFrom(s => s.Desk != null ? s.Desk.Number : 0))
            .ForMember(d => d.UserFullName, opt => opt.MapFrom(s => s.User != null ? s.User.FullName : ""));
    }
}
