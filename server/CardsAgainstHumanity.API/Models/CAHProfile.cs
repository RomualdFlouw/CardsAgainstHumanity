using AutoMapper;
using CardsAgainstHumanity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Models
{
    public class CAHProfile : Profile
    {
        public CAHProfile()
        {
            CreateMap<Category, CategoryDTO>();

            CreateMap<Deck, DeckDTO>();

            CreateMap<Card, CardDTO>();
        }
    }
}
