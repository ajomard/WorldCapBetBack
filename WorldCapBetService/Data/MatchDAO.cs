using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Data
{
    public class MatchDAO
    {
        private readonly Context _context;

        public MatchDAO(Context context)
        {
            _context = context;
        }

        public IList<MatchWithPronosticViewModel> GetMatchesWithPronosticFromUser(string userId)
        {
            var results = new List<MatchWithPronosticViewModel>();
            var matches = _context.Match.Include("Team1").Include("Team2");
            var pronostics = _context.Pronostic.Where(p => p.User.Id == userId);

            foreach(var match in matches)
            {
                var result = new MatchWithPronosticViewModel();
                var pronostic = pronostics.SingleOrDefault(p => p.Match.Id == match.Id);

                result.Id = match.Id;
                result.Date = match.Date;
                result.Team1 = match.Team1;
                result.Team2 = match.Team2;
                result.ScoreTeam1 = match.ScoreTeam1;
                result.ScoreTeam2 = match.ScoreTeam2;
                result.Pronostic = pronostic;

                results.Add(result);
            }

            return results;

        }
    }
}
