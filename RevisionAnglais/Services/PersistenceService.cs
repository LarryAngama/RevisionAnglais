using RevisionAnglais.Models;
using System.IO;
using System.Text.Json;

namespace RevisionAnglais.Services
{
    public class PersistenceService
    {
        private readonly string _preferencesPath;

        public PersistenceService()
        {
            // Creer le dossier AppData pour stocker les preferences
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RevisionAnglais");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            _preferencesPath = Path.Combine(appDataPath, "preferences.json");
        }

        public void SavePreferences(List<VerbIrregulier> selectedVerbs)
        {
            try
            {
                var preferences = new UserPreferences
                {
                    SelectedVerbIds = selectedVerbs.Select(v => v.Id).ToList()
                };

                var json = JsonSerializer.Serialize(preferences, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_preferencesPath, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de la sauvegarde des preferences: {ex.Message}", "Erreur");
            }
        }

        public List<int> LoadPreferences()
        {
            try
            {
                if (!File.Exists(_preferencesPath))
                {
                    return new List<int>();
                }

                var json = File.ReadAllText(_preferencesPath);
                var preferences = JsonSerializer.Deserialize<UserPreferences>(json);
                return preferences?.SelectedVerbIds ?? new List<int>();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des preferences: {ex.Message}", "Erreur");
                return new List<int>();
            }
        }
    }
}
