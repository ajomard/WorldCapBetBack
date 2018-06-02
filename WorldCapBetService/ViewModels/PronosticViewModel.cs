using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBetService.ViewModels
{
    public class PronosticViewModel
    {
        public long Id { get; set; }
        public MatchViewModel Match { get; set; }
        public UserViewModel User { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }
    }
}
