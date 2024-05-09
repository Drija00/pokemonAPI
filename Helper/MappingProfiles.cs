using AutoMapper;
using test.dto;
using test.Models;

namespace test
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon,PokemonDTO>();
            CreateMap<Category,CategoryDTO>();
            CreateMap<CategoryPostDTO,Category>();
            CreateMap<Country,CountryDTO>();
            CreateMap<CountryPostDTO,Country>();
            CreateMap<Owner,OwnerDTO>();
            CreateMap<OwnerPostDTO,Owner>();
            CreateMap<PokemonDTO,Pokemon>();
            CreateMap<PokemonPostDTO,Pokemon>();
            CreateMap<Review,ReviewDTO>();
            CreateMap<ReviewDTO,Review>();
            CreateMap<Reviewer,ReviewerDTO>();
            CreateMap<ReviewerPostDTO,Reviewer>();
        }
    }
}