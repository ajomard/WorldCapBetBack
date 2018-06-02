using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.BLL
{
    public class MatchManager
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public MatchManager(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<MatchWithPronosticViewModel> GetMatchesWithPronosticFromUser(string userId)
        {
            var results = new List<MatchWithPronosticViewModel>();
            var matches = _context.Match.Include("Team1").Include("Team2").ToList();
            var pronostics = _context.Pronostic.Include("User").Include("Match").Where(p => p.User.Id == userId).ToList();
            var pronosticsVM = _mapper.Map<IList<PronosticViewModel>>(pronostics);

            var result = _mapper.Map<IList<MatchWithPronosticViewModel>>(matches);
            foreach (var match in result)
            {
                match.Date = match.Date.ToUniversalTime();
                match.Pronostic = pronosticsVM.SingleOrDefault(p => p.Match.Id == match.Id);
            }

            return result;

        }

        public IList<MatchWithPronosticViewModel> GetTodayMatchesWithPronosticFromUser(string userId)
        {
            var results = new List<MatchWithPronosticViewModel>();
            var matches = _context.Match.Include("Team1").Include("Team2").Where(m => m.Date >= DateTime.Today && m.Date < DateTime.Today.AddDays(1)).ToList();
            var pronostics = _context.Pronostic.Include("User").Include("Match").Where(p => p.User.Id == userId).ToList();
            var pronosticsVM = _mapper.Map<IList<PronosticViewModel>>(pronostics);

            var result = _mapper.Map<IList<MatchWithPronosticViewModel>>(matches);
            foreach (var match in result)
            {
                match.Date = match.Date.ToUniversalTime();
                match.Pronostic = pronosticsVM.SingleOrDefault(p => p.Match.Id == match.Id);
            }

            return result;

        }
    }
}
