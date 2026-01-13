namespace RevisionAnglais.Models
{
    public class VerbIrregulier
    {
        public int Id { get; set; }
        public string Infinitif { get; set; }
        public string PastSimple { get; set; }
        public string Traduction { get; set; }

        public VerbIrregulier(int id, string infinitif, string pastSimple, string traduction)
        {
            Id = id;
            Infinitif = infinitif;
            PastSimple = pastSimple;
            Traduction = traduction;
        }

        public VerbIrregulier()
        {
        }

        public override string ToString()
        {
            return $"{Infinitif} - {PastSimple} ({Traduction})";
        }
    }
}
