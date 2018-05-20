using System.Collections.Generic;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.ViewModels
{
    public class ChartAverageStats
    {
        /*public Ranking UserStats { get; set; }

        public double AverageScore { get; set; }
        public double AverageFalsePronostic { get; set; }
        public double AverageGoodPronosticOnly { get; set; }
        public double AverageGoodGoalAverage { get; set; }
        public double AverageGoodPronosticAndGoodScore { get; set; }*/

        public IList<GroupingBar> BarList { get; set; }


    }

    public class GroupingBar
    {
        public string Name { get; set; }
        public IList<Bar> Series { get; set; }

    }

    public class Bar
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public Bar()
        {

        }

        public Bar(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
