using RevisionAnglais.Models;
using System.Windows;

namespace RevisionAnglais
{
    public partial class ModeSelectionWindow : Window
    {
        private List<VerbIrregulier> _selectedVerbs;

        public ModeSelectionWindow(List<VerbIrregulier> selectedVerbs)
        {
            InitializeComponent();
            _selectedVerbs = selectedVerbs;
        }

        private void ClassicMode_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var exerciseWindow = new ExcerciseWindows(_selectedVerbs, false);
            exerciseWindow.Show();
            this.Close();
        }

        private void AdvancedMode_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var advancedWindow = new AdvancedExerciseWindows(_selectedVerbs);
            advancedWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
