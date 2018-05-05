namespace WorldCapBetService.Models.Entities
{
    public class Pronostic
    {
        public long Id { get; set; }
        public Match Match { get; set; }
        public User User { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }
    }
}
