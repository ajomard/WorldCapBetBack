using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Data
{
    public class ChartsManager
    {
        private readonly Context _context;

        public ChartsManager(Context context)
        {
            _context = context;
        }

        public ChartAverageStats GetAverageStatsFromUser(string userId)
        {
            var user = _context.User.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                //chartAverageStats.UserStats = _context.Rankings.SingleOrDefault(r => r.User.Id == userId);
                var allRankings = _context.Rankings.ToList();
                var averageGoodPronosticAndGoodScore = allRankings.Average(r => r.GoodPronosticAndGoodScore);
                var averageGoodGoalAverage = _context.Rankings.Average(r => r.GoodGoalAverage);
                var averageGoodPronosticOnly = _context.Rankings.Average(r => r.GoodPronosticOnly);
                var averageFalsePronostic = _context.Rankings.Average(r => r.FalsePronostic);
                var averageScore = _context.Rankings.Average(r => r.Score);

                var chartAverageStats = new ChartAverageStats();
                var userStats = _context.Rankings.SingleOrDefault(r => r.User.Id == userId);

                chartAverageStats.BarList = new List<GroupingBar>();
                //chartAverageStats.BarList.Add(CreateAverageStatBar(userStats.Score, averageScore, "Score", user));
                chartAverageStats.BarList.Add(CreateAverageStatBar(userStats.GoodPronosticAndGoodScore, averageGoodPronosticAndGoodScore, "Good Pronostic And Good Score", user));
                chartAverageStats.BarList.Add(CreateAverageStatBar(userStats.GoodGoalAverage, averageGoodGoalAverage, "Good Goal Average", user));
                chartAverageStats.BarList.Add(CreateAverageStatBar(userStats.GoodPronosticOnly, averageGoodPronosticOnly, "Good Pronostic Only", user));
                chartAverageStats.BarList.Add(CreateAverageStatBar(userStats.FalsePronostic, averageFalsePronostic, "False Pronostic", user));

                return chartAverageStats;
            }
            return null;
        }

        private GroupingBar CreateAverageStatBar(int userStat, double averageStat, string groupingName, User user)
        {
            var userName = user.FirstName + " " + user.LastName;

            var groupingBar = new GroupingBar();
            groupingBar.Name = groupingName;
            groupingBar.Series = new List<Bar>();
            var userBar = new Bar(userName, userStat);
            var averageBar = new Bar("Average", averageStat);

            groupingBar.Series.Add(userBar);
            groupingBar.Series.Add(averageBar);

            return groupingBar;
        }

    }
}
