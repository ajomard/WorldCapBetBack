using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Data
{
    public class RankingManager
    {
        private readonly Context _context;

        public RankingManager(Context context)
        {
            _context = context;
        }

        public void UpdateRankings()
        {
            DeleteAllRankings();

            var userList = _context.User;
            var rankList = new List<Ranking>();
            foreach (var user in userList)
            {
                rankList.Add(CaculateScoreFromOneUser(user));
            }

            rankList = rankList.OrderByDescending(r => r.Score).ToList();
            var rank = 1;
            foreach (var ranking in rankList)
            {
                ranking.Rank = rank;
                rank++;
            }

            _context.Rankings.AddRange(rankList);
            _context.SaveChanges();

        }

        private Ranking CaculateScoreFromOneUser(User user)
        {
            var listPronostics = _context.Pronostic.Include("Match").Where(p => p.User.Id == user.Id && p.Match.ScoreTeam1 != null && p.Match.ScoreTeam2 != null);
            var userScore = new Ranking();
            userScore.User = user;
            foreach (var pronostic in listPronostics)
            {
                var goalAverage = Math.Abs((decimal)pronostic.Match.ScoreTeam1 - (decimal)pronostic.Match.ScoreTeam2);
                var goalAveragePronostic = Math.Abs((decimal)pronostic.ScoreTeam1 - (decimal)pronostic.ScoreTeam2);
                var isTeam1Winner = pronostic.Match.ScoreTeam1 > pronostic.Match.ScoreTeam2;
                var isPronosticTeam1Winner = pronostic.ScoreTeam1 > pronostic.ScoreTeam2;
                var isTeam2Winner = pronostic.Match.ScoreTeam1 < pronostic.Match.ScoreTeam2;
                var isPronosticTeam2Winner = pronostic.ScoreTeam1 < pronostic.ScoreTeam2;
                var isDraw = pronostic.Match.ScoreTeam1 == pronostic.Match.ScoreTeam2;
                var isDrawPronostic = pronostic.ScoreTeam1 == pronostic.ScoreTeam2;



                if (pronostic.Match.ScoreTeam1 == pronostic.ScoreTeam1 && pronostic.Match.ScoreTeam2 == pronostic.ScoreTeam2)
                {
                    userScore.GoodPronosticAndGoodScore++;
                }
                else if ((isTeam1Winner && isPronosticTeam1Winner || isTeam2Winner && isPronosticTeam2Winner || isDraw && isDrawPronostic) && goalAverage == goalAveragePronostic)
                {
                    userScore.GoodGoalAverage++;
                }
                else if (isTeam1Winner && isPronosticTeam1Winner || isTeam2Winner && isPronosticTeam2Winner)
                {
                    userScore.GoodPronosticOnly++;
                }
                else
                {
                    userScore.FalsePronostic++;
                }
            }

            userScore.Score = userScore.GoodPronosticAndGoodScore * 4 + userScore.GoodGoalAverage * 2 + userScore.GoodPronosticOnly;

            return userScore;

        }


        private void DeleteAllRankings()
        {
            var listRankings = _context.Rankings;
            _context.Rankings.RemoveRange(listRankings);
            _context.SaveChanges();
        }

    }
}
