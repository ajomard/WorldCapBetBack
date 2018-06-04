using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.BLL
{
    public class RankingManager
    {
        private readonly Context _context;

        public RankingManager(Context context)
        {
            _context = context;
        }

        public IList<Ranking> GetAroundUserRanking(string userId)
        {
            var ranking = _context.Rankings.SingleOrDefault(r => r.User.Id == userId);

            if (ranking == null)
            {
                return new List<Ranking>();
            }

            int min;
            int max;
            var userRank = ranking.Rank;
            var lastRank = _context.Rankings.Max(r => r.Rank);
            if (userRank == 1 || userRank == 2 || userRank == 3)
            {
                min = 1;
                max = 5;
            }
            else if (userRank == lastRank || userRank == lastRank - 1 || userRank == lastRank - 2)
            {
                min = lastRank - 4;
                max = lastRank;
            }
            else
            {
                min = userRank - 2;
                max = userRank + 2;
            }

            var rankings = _context.Rankings.Include("User").Where(r => r.Rank >= min && r.Rank <= max).OrderBy(r => r.Rank).ToList();

            return rankings;
        }

        public void UpdateRankings()
        {
            DeleteAllRankings();

            var userList = _context.User;
            var rankList = new List<Ranking>();
            foreach (var user in userList)
            {
                var userStats = CaculateScoreFromOneUser(user);
                if (userStats != null)
                {
                    rankList.Add(userStats);
                }
            }

            if (rankList.Count > 0)
            {
                rankList = rankList.OrderByDescending(x => x.Score).ThenByDescending(x => x.GoodPronosticAndGoodScore).ThenByDescending(x => x.GoodGoalAverage)
               .ThenByDescending(x => x.GoodPronosticOnly).ToList();
                var rank = 1;
                foreach (var ranking in rankList)
                {
                    ranking.Rank = rank;
                    rank++;
                }

                _context.Rankings.AddRange(rankList);
                _context.SaveChanges();
            }

        }

        private Ranking CaculateScoreFromOneUser(User user)
        {
            var listPronostics = _context.Pronostic.Include("Match").Where(p => p.User.Id == user.Id && p.Match.ScoreTeam1 != null && p.Match.ScoreTeam2 != null).ToList();
            if (listPronostics.Count == 0)
            {
                //no pronostic with ended match for this user
                return null;
            }

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
