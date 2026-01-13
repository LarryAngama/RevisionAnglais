using RevisionAnglais.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevisionAnglais
{
    public partial class ExcerciseWindows : Window
    {
        private List<VerbIrregulier> _verbs;
        private List<VerbIrregulier> _allVerbs;
        private Random _random;
        private int _correctAnswers;
        private int _totalAnswers;
        private VerbIrregulier _currentVerb;
        private List<VerbIrregulier> _options;
        private List<Answer> _allAnswers;
        private int _questionCount;

        public ExcerciseWindows(List<VerbIrregulier> verbs)
        {
            InitializeComponent();
            _random = new Random();
            _allVerbs = new List<VerbIrregulier>(verbs);
            _verbs = verbs.OrderBy(x => _random.Next()).ToList();
            _correctAnswers = 0;
            _totalAnswers = 0;
            _allAnswers = new List<Answer>();
            _questionCount = _verbs.Count;
            _currentVerb = _verbs.Count > 0 ? _verbs[0] : new VerbIrregulier(0, string.Empty, string.Empty, string.Empty);
            _options = new List<VerbIrregulier>();
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            if (_verbs.Count == 0)
            {
                // Fin du test - afficher les resultats
                ShowResults();
                return;
            }

            _currentVerb = _verbs[_random.Next(_verbs.Count)];
            _options = GenerateOptions(_currentVerb);

            // Afficher la question
            int questionType = _random.Next(3);
            switch (questionType)
            {
                case 0: // Infinitif -> Past Simple
                    QuestionText.Text = $"Quel est le Past Simple de '{_currentVerb.Infinitif}' ?";
                    break;
                case 1: // Traduction -> Past Simple
                    QuestionText.Text = $"Traduisez et conjuguez : '{_currentVerb.Traduction}' (infinitif au past simple) ?";
                    break;
                case 2: // Past Simple -> Traduction
                    QuestionText.Text = $"Que signifie '{_currentVerb.PastSimple}' (past simple) ?";
                    break;
            }

            // Melanger les options
            _options = _options.OrderBy(x => _random.Next()).ToList();

            // Afficher les boutons de reponse
            AnswerPanel.Children.Clear();
            foreach (var option in _options)
            {
                var button = new System.Windows.Controls.Button
                {
                    Content = GetOptionDisplay(option, questionType),
                    Padding = new Thickness(15),
                    Margin = new Thickness(5),
                    FontSize = 14,
                    Tag = option
                };
                button.Click += AnswerButton_Click;
                AnswerPanel.Children.Add(button);
            }

            UpdateProgress();
        }

        private string GetOptionDisplay(VerbIrregulier verb, int questionType)
        {
            return questionType switch
            {
                0 => verb.PastSimple,
                1 => verb.PastSimple,
                2 => verb.Traduction,
                _ => ""
            };
        }

        private List<VerbIrregulier> GenerateOptions(VerbIrregulier correct)
        {
            var options = new List<VerbIrregulier> { correct };
            var otherVerbs = _allVerbs.Where(v => v.Id != correct.Id).OrderBy(x => _random.Next()).Take(3).ToList();
            options.AddRange(otherVerbs);
            return options;
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            var selectedVerb = (VerbIrregulier)button.Tag;
            var isCorrect = selectedVerb.Id == _currentVerb.Id;

            // Enregistrer la reponse
            _allAnswers.Add(new Answer(_currentVerb, isCorrect));

            if (isCorrect)
            {
                _correctAnswers++;
                button.Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)); // Vert
                MessageBox.Show($"Correct! 🎉\", \"\r\r Verbe à l'infinitif:{_currentVerb.Infinitif} \r Past simple: {_currentVerb.PastSimple} \r Traduction: {_currentVerb.Traduction}", "Bravo !");
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(244, 67, 54)); // Rouge
                MessageBox.Show($"Incorrect! La reponse etait: \r\r Verbe à l'infinitif:{_currentVerb.Infinitif} \r Past simple: {_currentVerb.PastSimple} \r Traduction: {_currentVerb.Infinitif}", "Essayez encore");
            }

            _totalAnswers++;
            
            // Supprimer le verbe de la liste des questions
            _verbs.Remove(_currentVerb);
            
            // Charger la question suivante ou afficher les resultats
            LoadNextQuestion();
        }

        private void UpdateProgress()
        {
            ProgressText.Text = $"Question {_totalAnswers + 1}/{_questionCount}";
            if (_totalAnswers > 0)
            {
                double percentage = (_correctAnswers * 100.0) / _totalAnswers;
                ScoreText.Text = $"Score actuel: {percentage:F1}%";
            }
            else
            {
                ScoreText.Text = "Score actuel: 0%";
            }
        }

        private void ShowResults()
        {
            ResultsWindow resultsWindow = new ResultsWindow(_allAnswers);
            resultsWindow.Show();
            this.Close();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

