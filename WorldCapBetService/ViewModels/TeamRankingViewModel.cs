using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBetService.ViewModels
{
    public class TeamRankingViewModel
    {
        public TeamViewModel Team { get; set; }
        public int Score { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Loose { get; set; }
        public int GoalAverage { get; set; }

        public TeamRankingViewModel() { }

        public TeamRankingViewModel(TeamViewModel team)
        {
            this.Team = team;
        }

    }
}
