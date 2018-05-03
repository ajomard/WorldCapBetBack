using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBetService.Models.Entities
{
    public class Match
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public Team Team1 { get; set; }
        [Required]
        public Team Team2 { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }

        public ICollection<Pronostic> Pronostics { get; set; }
    }
}
