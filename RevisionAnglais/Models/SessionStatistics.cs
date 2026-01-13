namespace RevisionAnglais.Models
{
    public class SessionStatistics
    {
        public DateTime Date { get; set; }
        public int TotalVerbs { get; set; }
        public int CorrectAnswers { get; set; }
        public double SuccessRate { get; set; }

        public SessionStatistics(int totalVerbs, int correctAnswers, double successRate)
        {
            Date = DateTime.Now;
            TotalVerbs = totalVerbs;
            CorrectAnswers = correctAnswers;
            SuccessRate = successRate;
        }

        public override string ToString()
        {
            return $"{Date:dd/MM/yyyy HH:mm} - {CorrectAnswers}/{TotalVerbs} ({SuccessRate:F1}%)";
        }
    }
}
