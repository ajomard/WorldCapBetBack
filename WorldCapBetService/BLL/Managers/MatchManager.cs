using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldCapBetService.Models;
using WorldCapBetService.Models.Entities;
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

        public IList<TeamRankingViewModel> GetTeamRankingOfGroup(string group)
        {
            var resultList = new List<TeamRankingViewModel>();
            var teams = new Dictionary<long, TeamRankingViewModel>();
            var groupMatches = _context.Match.Include("Team1").Include("Team2")
                .Where(m => m.Team1.Group == group && m.Team2.Group == group && m.Type == EnumMatchType.Group).ToList();

            foreach (Match match in groupMatches)
            {
                var team1 = teams.GetValueOrDefault(match.Team1.Id);
                if (team1 == null)
                {
                    team1 = new TeamRankingViewModel(_mapper.Map<TeamViewModel>(match.Team1));

                }

                var team2 = teams.GetValueOrDefault(match.Team2.Id);
                if (team2 == null)
                {
                    team2 = new TeamRankingViewModel(_mapper.Map<TeamViewModel>(match.Team2));

                }

                if(match.ScoreTeam1 != null && match.ScoreTeam2 != null)
                {
                    if (match.ScoreTeam1 > match.ScoreTeam2)
                    {
                        team1.Win++;
                        team2.Loose++;
                        team1.Score = team1.Score + 3;
                    }
                    else if (match.ScoreTeam1 < match.ScoreTeam2)
                    {
                        team1.Loose++;
                        team2.Win++;
                        team2.Score = team2.Score + 3;
                    }
                    else
                    {
                        team1.Draw++;
                        team2.Draw++;
                        team1.Score++;
                        team2.Score++;
                    }
                    team1.GoalAverage = team1.GoalAverage + (int)match.ScoreTeam1 - (int)match.ScoreTeam2;
                    team2.GoalAverage = team2.GoalAverage + (int)match.ScoreTeam2 - (int)match.ScoreTeam1;
                }

                AddOrUpdateValue(teams, team1);
                AddOrUpdateValue(teams, team2);

                

            }

            return teams.Values.OrderByDescending(r => r.Score).ThenByDescending(r => r.GoalAverage).ToList();
        }

        private static void AddOrUpdateValue(Dictionary<long, TeamRankingViewModel> dictionnary, TeamRankingViewModel value)
        {
            if (dictionnary.ContainsKey(value.Team.Id))
            {
                dictionnary[value.Team.Id] = value;
            }
            else
            {
                dictionnary.Add(value.Team.Id, value);
            }
        }
    }
}
