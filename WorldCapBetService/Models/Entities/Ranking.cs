using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBetService.Models.Entities
{
    public class Ranking
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
        public int FalsePronostic { get; set; }
        public int GoodPronosticOnly { get; set; }
        public int GoodGoalAverage { get; set; }
        public int GoodPronosticAndGoodScore { get; set; }


    }
}
