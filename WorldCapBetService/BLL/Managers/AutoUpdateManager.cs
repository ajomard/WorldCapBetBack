using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WorldCapBetService.BLL.Managers
{
    public class AutoUpdateManager
    {
        private readonly Context _context;
        private readonly ApiFootballDataClient _client;

        public AutoUpdateManager(Context context, ApiFootballDataClient client)
        {
            _context = context;
            _client = client;
        }

        public async Task<bool> AutoUpdateScores()
        {
            var isMatchUpdated = false;
            //add 100 min => end of match
            //var matchesToBeUpdated = _context.Match.Include("Team1").Include("Team2").Where(m => m.ScoreTeam1 == null && m.ScoreTeam2 == null && m.Date.ToLocalTime().AddMinutes(100) < DateTime.Now);
            var matchesToBeUpdated = _context.Match.Include("Team1").Include("Team2").Where(m => m.ScoreTeam1 == null && m.ScoreTeam2 == null && m.Date.Day == DateTime.Today.Day && m.Date.Month == DateTime.Today.Month);
            if (matchesToBeUpdated.Any())
            {
                var apiDatas = await _client.GetFixtures();

                foreach (var matchToUpdate in matchesToBeUpdated)
                {
                    //only one match
                    var fixturesForMatch = apiDatas.Fixtures.SingleOrDefault(f => f.Result.GoalsAwayTeam != null && f.Result.GoalsHomeTeam != null
                                                && f.HomeTeamName == matchToUpdate.Team1.Name
                                                && f.AwayTeamName == matchToUpdate.Team2.Name);
                    if (fixturesForMatch != null)
                    {
                        matchToUpdate.ScoreTeam1 = (int)fixturesForMatch.Result.GoalsHomeTeam;
                        matchToUpdate.ScoreTeam2 = (int)fixturesForMatch.Result.GoalsAwayTeam;
                        _context.Entry(matchToUpdate).State = EntityState.Modified;
                        isMatchUpdated = true;
                    }
                }
                if (isMatchUpdated)
                {
                    await _context.SaveChangesAsync();
                }

            }
            return isMatchUpdated;
        }
    }
}
