namespace RevisionAnglais.Models
{
    public class UserPreferences
    {
        public List<int> SelectedVerbIds { get; set; }

        public UserPreferences()
        {
            SelectedVerbIds = new List<int>();
        }
    }
}
