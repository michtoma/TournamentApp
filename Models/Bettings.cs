namespace Mundial.Models
{
    public class Bettings
    {

        public int Id { get; set; }
        public AppUsers? User { get; set; }
        public string? UserID { get; set; }
        public Game? Game { get; set; }
        public int GameId { get; set; }
        public int ScoreTeam1 { get; set; } = 0;
        public int ScoreTeam2 { get; set; } = 0;
        public int BetPoints { get; set;} =0;
        
    }
}
