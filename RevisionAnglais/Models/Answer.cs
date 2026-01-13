namespace RevisionAnglais.Models
{
    public class Answer
    {
        public VerbIrregulier Verb { get; set; }
        public bool IsCorrect { get; set; }

        public Answer(VerbIrregulier verb, bool isCorrect)
        {
            Verb = verb;
            IsCorrect = isCorrect;
        }
    }
}
