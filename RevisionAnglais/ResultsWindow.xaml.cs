using RevisionAnglais.Models;
using RevisionAnglais.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevisionAnglais
{
    public partial class ResultsWindow : Window
    {
        public int CorrectCount { get; set; }
        public int TotalCount { get; set; }
        public double Percentage { get; set; }

        public ResultsWindow(List<Answer> answers)
        {
            InitializeComponent();
            
            TotalCount = answers.Count;
            CorrectCount = answers.Count(a => a.IsCorrect);
            Percentage = TotalCount > 0 ? (CorrectCount * 100.0) / TotalCount : 0;

            UpdateUI(answers);
            
            // Sauvegarder les statistiques
            SaveStatistics();
        }

        private void UpdateUI(List<Answer> answers)
        {
            ScoreTitleText.Text = $"Votre Score: {CorrectCount}/{TotalCount}";
            PercentageText.Text = $"{Percentage:F1}%";
            
            if (Percentage >= 80)
            {
                FeedbackText.Text = "Excellent travail! Vous maitrisez bien ces verbes!";
                FeedbackText.Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80));
            }
            else if (Percentage >= 60)
            {
                FeedbackText.Text = "Bon travail! Continuez vos efforts!";
                FeedbackText.Foreground = new SolidColorBrush(Color.FromRgb(255, 152, 0));
            }
            else
            {
                FeedbackText.Text = "Gardez courage! Revisez et essayez a nouveau!";
                FeedbackText.Foreground = new SolidColorBrush(Color.FromRgb(244, 67, 54));
            }

            // Construire la liste des resultats
            foreach (var answer in answers)
            {
                var resultBorder = new Border
                {
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(238, 238, 238)),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0),
                    Background = answer.IsCorrect 
                        ? new SolidColorBrush(Color.FromRgb(232, 245, 233))
                        : new SolidColorBrush(Color.FromRgb(255, 243, 224))
                };

                var stackPanel = new StackPanel();

                // Icone et statut
                var statusPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
                var statusIcon = new TextBlock
                {
                    Text = answer.IsCorrect ? "? CORRECT" : "? INCORRECT",
                    FontWeight = FontWeights.Bold,
                    Foreground = answer.IsCorrect 
                        ? new SolidColorBrush(Color.FromRgb(76, 175, 80))
                        : new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                    FontSize = 12
                };
                statusPanel.Children.Add(statusIcon);
                stackPanel.Children.Add(statusPanel);

                // Infinitif
                var infinitifPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 5) };
                infinitifPanel.Children.Add(new TextBlock { Text = "Infinitif: ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)) });
                infinitifPanel.Children.Add(new TextBlock { Text = answer.Verb.Infinitif, Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)) });
                stackPanel.Children.Add(infinitifPanel);

                // Past Simple
                var pastSimplePanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 5) };
                pastSimplePanel.Children.Add(new TextBlock { Text = "Past Simple: ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)) });
                pastSimplePanel.Children.Add(new TextBlock { Text = answer.Verb.PastSimple, Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)) });
                stackPanel.Children.Add(pastSimplePanel);

                // Traduction
                var traductionPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 5) };
                traductionPanel.Children.Add(new TextBlock { Text = "Traduction: ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)) });
                traductionPanel.Children.Add(new TextBlock { Text = answer.Verb.Traduction, Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)), FontStyle = FontStyles.Italic });
                stackPanel.Children.Add(traductionPanel);

                resultBorder.Child = stackPanel;
                ResultsPanel.Children.Add(resultBorder);
            }
        }

        private void SaveStatistics()
        {
            try
            {
                var statisticsService = new StatisticsService();
                var session = new SessionStatistics(TotalCount, CorrectCount, Percentage);
                statisticsService.SaveSession(session);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde des statistiques: {ex.Message}", "Erreur");
            }
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
