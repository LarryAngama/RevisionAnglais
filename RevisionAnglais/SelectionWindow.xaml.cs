using RevisionAnglais.Models;
using RevisionAnglais.Services;
using System.Windows;
using System.Windows.Controls;

namespace RevisionAnglais
{
    public partial class SelectionWindow : Window
    {
        private VerbService _verbService;
        private PersistenceService _persistenceService;
        private List<VerbIrregulier> _selectedVerbs;
        private List<int> _savedVerbIds;

        public SelectionWindow()
        {
            InitializeComponent();
            _verbService = new VerbService();
            _persistenceService = new PersistenceService();
            _selectedVerbs = new List<VerbIrregulier>();
            _savedVerbIds = _persistenceService.LoadPreferences();
            InitializeUI();
        }

        private void InitializeUI()
        {
            var verbs = _verbService.GetAllVerbs();
            foreach (var verb in verbs)
            {
                var checkBox = new CheckBox
                {
                    Content = verb.ToString(),
                    Tag = verb,
                    Margin = new Thickness(5),
                    IsChecked = _savedVerbIds.Contains(verb.Id)
                };
                VerbsPanel.Children.Add(checkBox);
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in VerbsPanel.Children)
            {
                cb.IsChecked = true;
            }
        }

        private void DeselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb in VerbsPanel.Children)
            {
                cb.IsChecked = false;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedVerbs.Clear();
            foreach (CheckBox cb in VerbsPanel.Children)
            {
                if (cb.IsChecked == true)
                {
                    _selectedVerbs.Add((VerbIrregulier)cb.Tag);
                }
            }

            if (_selectedVerbs.Count == 0)
            {
                MessageBox.Show("Veuillez selectionner au moins un verbe!", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Sauvegarder la selection
            _persistenceService.SavePreferences(_selectedVerbs);

            // Afficher la fenetre de selection du mode
            ModeSelectionWindow modeWindow = new ModeSelectionWindow(_selectedVerbs);
            modeWindow.Show();
            this.Close();
        }
    }
}
