using RevisionAnglais.Models;
using System.IO;
using System.Text.Json;

namespace RevisionAnglais.Services
{
    public class StatisticsService
    {
        private readonly string _statisticsPath;

        public StatisticsService()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RevisionAnglais");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            _statisticsPath = Path.Combine(appDataPath, "statistics.json");
        }

        public void SaveSession(SessionStatistics session)
        {
            try
            {
                var sessions = LoadAllSessions();
                sessions.Add(session);

                var json = JsonSerializer.Serialize(sessions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_statisticsPath, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de la sauvegarde des statistiques: {ex.Message}", "Erreur");
            }
        }

        public List<SessionStatistics> LoadAllSessions()
        {
            try
            {
                if (!File.Exists(_statisticsPath))
                {
                    return new List<SessionStatistics>();
                }

                var json = File.ReadAllText(_statisticsPath);
                var sessions = JsonSerializer.Deserialize<List<SessionStatistics>>(json);
                return sessions ?? new List<SessionStatistics>();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des statistiques: {ex.Message}", "Erreur");
                return new List<SessionStatistics>();
            }
        }

        public void ClearAllStatistics()
        {
            try
            {
                if (File.Exists(_statisticsPath))
                {
                    File.Delete(_statisticsPath);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de la suppression des statistiques: {ex.Message}", "Erreur");
            }
        }
    }
}
