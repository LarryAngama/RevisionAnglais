using RevisionAnglais.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevisionAnglais
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StatisticsService _statisticsService;

        public MainWindow()
        {
            InitializeComponent();
            _statisticsService = new StatisticsService();
            LoadHistory();
        }

        private void LoadHistory()
        {
            var sessions = _statisticsService.LoadAllSessions();
            HistoryPanel.Children.Clear();

            if (sessions.Count == 0)
            {
                var noDataText = new TextBlock
                {
                    Text = "Aucune revision pour le moment.",
                    Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153)),
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                HistoryPanel.Children.Add(noDataText);
                return;
            }

            // Afficher en ordre inverse (plus recent en premier)
            foreach (var session in sessions.OrderByDescending(s => s.Date))
            {
                var sessionBorder = new Border
                {
                    Background = GetBackgroundColor(session.SuccessRate),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Padding = new Thickness(10, 8, 10, 8),
                    Margin = new Thickness(0, 0, 0, 5)
                };

                var stackPanel = new StackPanel();

                // Date et score
                var scorePanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 3) };
                var dateText = new TextBlock
                {
                    Text = session.Date.ToString("dd/MM/yyyy HH:mm"),
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51))
                };
                scorePanel.Children.Add(dateText);
                stackPanel.Children.Add(scorePanel);

                // Verbes testés
                var verbsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 2) };
                verbsPanel.Children.Add(new TextBlock 
                { 
                    Text = $"Verbes: ", 
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102))
                });
                verbsPanel.Children.Add(new TextBlock 
                { 
                    Text = $"{session.TotalVerbs}", 
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102))
                });
                stackPanel.Children.Add(verbsPanel);

                // Réussite
                var ratePanel = new StackPanel { Orientation = Orientation.Horizontal };
                ratePanel.Children.Add(new TextBlock 
                { 
                    Text = $"Score: ", 
                    FontSize = 10,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102))
                });
                ratePanel.Children.Add(new TextBlock 
                { 
                    Text = $"{session.CorrectAnswers}/{session.TotalVerbs} ({session.SuccessRate:F1}%)", 
                    FontSize = 10,
                    Foreground = GetScoreColor(session.SuccessRate),
                    FontWeight = FontWeights.Bold
                });
                stackPanel.Children.Add(ratePanel);

                sessionBorder.Child = stackPanel;
                HistoryPanel.Children.Add(sessionBorder);
            }
        }

        private SolidColorBrush GetBackgroundColor(double successRate)
        {
            if (successRate >= 80)
                return new SolidColorBrush(Color.FromRgb(232, 245, 233));
            else if (successRate >= 60)
                return new SolidColorBrush(Color.FromRgb(255, 243, 224));
            else
                return new SolidColorBrush(Color.FromRgb(255, 235, 238));
        }

        private SolidColorBrush GetScoreColor(double successRate)
        {
            if (successRate >= 80)
                return new SolidColorBrush(Color.FromRgb(76, 175, 80));
            else if (successRate >= 60)
                return new SolidColorBrush(Color.FromRgb(255, 152, 0));
            else
                return new SolidColorBrush(Color.FromRgb(244, 67, 54));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SelectionWindow selectionWindow = new SelectionWindow();
            selectionWindow.Show();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Êtes-vous sur de vouloir effacer tout l'historique?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _statisticsService.ClearAllStatistics();
                LoadHistory();
                MessageBox.Show("Historique efface!", "Succes");
            }
        }
    }
}