using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBetService.Models;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.ViewModels
{
    public class MatchViewModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }
        public EnumMatchType? Type { get; set; }
        public string Title { get; set; }
        public EnumMatchStatus? Status { get; set; }
    }
}
