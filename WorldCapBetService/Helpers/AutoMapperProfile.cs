using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Match, MatchViewModel>();
            CreateMap<Match, MatchWithPronosticViewModel>();
            CreateMap<Team, TeamViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<Pronostic, PronosticViewModel>();
            CreateMap<Ranking, RankingViewModel>();
        }
    }
}
