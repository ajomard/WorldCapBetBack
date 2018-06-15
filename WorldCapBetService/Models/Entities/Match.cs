using System;
using System.Collections.Generic;

namespace WorldCapBetService.Models.Entities
{
    public class Match
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }
        public EnumMatchType? Type { get; set; }
        public string Title { get; set; }
        public EnumMatchStatus? Status { get; set; }

        public ICollection<Pronostic> Pronostics { get; set; }
    }
}
