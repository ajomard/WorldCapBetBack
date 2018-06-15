using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Models;
using WorldCapBetService.Models.Entities;

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
            // var listMatchesFinished = new List<bool>();
            var updateRanking = false;
            var updateDb = false;
            var matchesToBeUpdated = _context.Match.Include("Team1").Include("Team2").Where(m => m.Date.Day == DateTime.Today.Day && m.Date.Month == DateTime.Today.Month);
            if (matchesToBeUpdated.Any())
            {
                var apiDatas = await _client.GetFixtures();

                foreach (var matchToUpdate in matchesToBeUpdated)
                {
                    //only one match
                    var fixturesForMatch = apiDatas.Fixtures.SingleOrDefault(f => f.Result.GoalsAwayTeam != null && f.Result.GoalsHomeTeam != null
                                                && f.HomeTeamName == matchToUpdate.Team1.Name
                                                && f.AwayTeamName == matchToUpdate.Team2.Name
                                                && (f.Status == Status.InPlay || f.Status == Status.Finished));

                    if (fixturesForMatch != null && (IsScoreDifferent(matchToUpdate, fixturesForMatch) || StatusChanged(matchToUpdate, fixturesForMatch)))
                    {
                        matchToUpdate.ScoreTeam1 = fixturesForMatch.Result.GoalsHomeTeam;
                        matchToUpdate.ScoreTeam2 = fixturesForMatch.Result.GoalsAwayTeam;
                        matchToUpdate.Status = ConvertFixtureStatusToModelStatus(fixturesForMatch);
                        //because score is sometime null in api when match beggining
                        if (matchToUpdate.ScoreTeam1 == null)
                            matchToUpdate.ScoreTeam1 = 0;
                        if (matchToUpdate.ScoreTeam2 == null)
                            matchToUpdate.ScoreTeam2 = 0;
                        _context.Entry(matchToUpdate).State = EntityState.Modified;
                        updateDb = true;
                        //all matchs must be finished to update ranking
                        //listMatchesFinished.Add(fixturesForMatch.Status == Status.Finished);
                    }
                }
                if (updateDb)
                {
                    await _context.SaveChangesAsync();
                    updateRanking = true;
                }

                /*updateRanking = true;
                //if all matchs not finished, do not update ranking
                if (listMatchesFinished.Any(m => false))
                {
                    updateRanking = false;
                }*/
            }
            return updateRanking;
        }

        private bool IsScoreDifferent(Match match, Fixture fixture)
        {
            return match.ScoreTeam1 != fixture.Result.GoalsHomeTeam || match.ScoreTeam2 != fixture.Result.GoalsAwayTeam;
        }

        private bool StatusChanged(Match match, Fixture fixture)
        {
            return match.Status != ConvertFixtureStatusToModelStatus(fixture);
        }

        private EnumMatchStatus ConvertFixtureStatusToModelStatus(Fixture fixture)
        {
            switch (fixture.Status)
            {
                case Status.InPlay:
                    return EnumMatchStatus.InPlay;
                case Status.Finished:
                    return EnumMatchStatus.Finished;
                default:
                    return EnumMatchStatus.NotStarted;
            }
        }
    }
}
