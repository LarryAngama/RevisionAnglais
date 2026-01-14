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
        private List<(VerbIrregulier verb, int questionType)> _questions;
        private int _currentQuestionIndex;
        private int _correctAnswers;
        private List<VerbIrregulier> _allVerbs;
        private Random _random;
        private VerbIrregulier _currentVerb;
        private List<VerbIrregulier> _options;
        private List<Answer> _allAnswers;
        private int _questionCount;
        private int _questionType;

        public ExcerciseWindows(List<VerbIrregulier> verbs, bool isAdvanced = false)
        {
            InitializeComponent();
            _random = new Random();
            _allVerbs = new List<VerbIrregulier>(verbs);
            _correctAnswers = 0;
            _allAnswers = new List<Answer>();
            _currentQuestionIndex = 0;
            
            // Créer la liste de questions : 3 types par verbe
            _questions = new List<(VerbIrregulier, int)>();
            
            foreach (var verb in verbs)
            {
                // Créer une liste de 3 types de questions [0, 1, 2]
                var questionTypes = new List<int> { 0, 1, 2 };
                
                // Mélanger les types de questions
                questionTypes = questionTypes.OrderBy(x => _random.Next()).ToList();
                
                // Ajouter chaque type de question pour ce verbe
                foreach (var type in questionTypes)
                {
                    _questions.Add((verb, type));
                }
            }
            
            // Mélanger l'ordre de toutes les questions
            _questions = _questions.OrderBy(x => _random.Next()).ToList();
            
            _questionCount = _questions.Count;
            _currentVerb = _questions.Count > 0 ? _questions[0].verb : new VerbIrregulier(0, string.Empty, string.Empty, string.Empty);
            _options = new List<VerbIrregulier>();
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                // Fin du test - afficher les resultats
                ShowResults();
                return;
            }

            var (verb, questionType) = _questions[_currentQuestionIndex];
            _currentVerb = verb;
            _questionType = questionType;
            
            _options = GenerateOptions(_currentVerb);

            // Afficher la question
            switch (_questionType)
            {
                case 0: // Infinitif -> Past Simple
                    QuestionText.Text = $"Quel est le Past Simple de '{_currentVerb.Infinitif}' ?";
                    break;
                case 1: // Traduction -> Past Simple
                    QuestionText.Text = $"Traduisez et conjuguez : '{_currentVerb.Traduction}' (au past simple) ?";
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
                    Content = GetOptionDisplay(option, _questionType),
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
                MessageBox.Show($"Correct! 🎉\nVerbe à l'infinitif: {_currentVerb.Infinitif}\nPast simple: {_currentVerb.PastSimple}\nTraduction: {_currentVerb.Traduction}", "Bravo !");
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(244, 67, 54)); // Rouge
                MessageBox.Show($"Incorrect! La reponse etait:\nVerbe à l'infinitif: {_currentVerb.Infinitif}\nPast simple: {_currentVerb.PastSimple}\nTraduction: {_currentVerb.Traduction}", "Essayez encore");
            }

            // Passer a la question suivante
            _currentQuestionIndex++;
            LoadNextQuestion();
        }

        private void UpdateProgress()
        {
            ProgressText.Text = $"Question {_currentQuestionIndex + 1}/{_questionCount}";
            if (_currentQuestionIndex > 0)
            {
                double percentage = (_correctAnswers * 100.0) / _currentQuestionIndex;
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

