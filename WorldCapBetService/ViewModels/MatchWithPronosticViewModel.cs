using System;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.ViewModels
{
    public class MatchWithPronosticViewModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }

        public Pronostic Pronostic { get; set; }
    }
}
