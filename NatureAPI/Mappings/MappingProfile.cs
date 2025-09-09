using AutoMapper;
using NatureAPI.Models.DTOs;
using NatureAPI.Models.Entities;

namespace NatureAPI.Mappings;

/// <summary>
/// Perfiles de AutoMapper para mapear entre entidades y DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeos para Place
        CreateMap<Place, PlaceListDto>();
        
        CreateMap<Place, PlaceDetailDto>()
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => 
                src.PlaceAmenities.Select(pa => pa.Amenity)));
        
        CreateMap<CreatePlaceDto, Place>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Trails, opt => opt.Ignore())
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.PlaceAmenities, opt => opt.Ignore());

        // Mapeos para Trail
        CreateMap<Trail, TrailDto>();

        // Mapeos para Photo
        CreateMap<Photo, PhotoDto>();

        // Mapeos para Review
        CreateMap<Review, ReviewDto>();

        // Mapeos para Amenity
        CreateMap<Amenity, AmenityDto>();
    }
}
