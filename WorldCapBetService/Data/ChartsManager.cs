﻿using System;
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

        public IList<Bar> GetAverageScoreFromUser(string userId)
        {
            var user = _context.User.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var allRankings = _context.Rankings.ToList();
                var averageScore = allRankings.Average(r => r.Score);

                var userStats = _context.Rankings.SingleOrDefault(r => r.User.Id == userId);

                var barList = new List<Bar>
                {
                    new Bar(user.FirstName + " " + user.LastName, userStats.Score),
                    new Bar("Average", averageScore)
                };

                return barList;
            }
            return null;
        }

        public IList<GroupingBar> GetAverageStatsFromUser(string userId)
        {
            var user = _context.User.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var allRankings = _context.Rankings.ToList();
                var averageGoodPronosticAndGoodScore = allRankings.Average(r => r.GoodPronosticAndGoodScore);
                var averageGoodGoalAverage = _context.Rankings.Average(r => r.GoodGoalAverage);
                var averageGoodPronosticOnly = _context.Rankings.Average(r => r.GoodPronosticOnly);
                var averageFalsePronostic = _context.Rankings.Average(r => r.FalsePronostic);
                var averageScore = _context.Rankings.Average(r => r.Score);

                var userStats = _context.Rankings.SingleOrDefault(r => r.User.Id == userId);

                var barList = new List<GroupingBar>
                {
                    CreateAverageStatBar(userStats.GoodPronosticAndGoodScore, averageGoodPronosticAndGoodScore, "Good Pronostic And Good Score", user),
                    CreateAverageStatBar(userStats.GoodGoalAverage, averageGoodGoalAverage, "Good Goal Average", user),
                    CreateAverageStatBar(userStats.GoodPronosticOnly, averageGoodPronosticOnly, "Good Pronostic Only", user),
                    CreateAverageStatBar(userStats.FalsePronostic, averageFalsePronostic, "False Pronostic", user)
                };

                return barList;
            }
            return null;
        }

        private GroupingBar CreateAverageStatBar(int userStat, double averageStat, string groupingName, User user)
        {
            var userName = user.FirstName + " " + user.LastName;

            var groupingBar = new GroupingBar
            {
                Name = groupingName,
                Series = new List<Bar>()
            };
            var userBar = new Bar(userName, userStat);
            var averageBar = new Bar("Average", averageStat);

            groupingBar.Series.Add(userBar);
            groupingBar.Series.Add(averageBar);

            return groupingBar;
        }

    }
}
