using System.Collections.Generic;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.ViewModels
{
    public class ChartAverageStatsViewModel
    {
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
