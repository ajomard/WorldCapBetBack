using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBetService.Models.Entities
{
    public class Pronostic
    {
        public long Id { get; set; }
        [Required]
        public Match Match { get; set; }
        [Required]
        public User User { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }
    }
}
