using RevisionAnglais.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevisionAnglais
{
    public partial class AdvancedExerciseWindows : Window
    {
        private List<(VerbIrregulier verb, int questionType)> _questions;
        private int _currentQuestionIndex;
        private int _correctAnswers;
        private VerbIrregulier _currentVerb;
        private List<Answer> _allAnswers;
        private int _questionCount;
        private int _questionType;
        private bool _answered;

        public AdvancedExerciseWindows(List<VerbIrregulier> verbs)
        {
            InitializeComponent();
            _currentQuestionIndex = 0;
            _correctAnswers = 0;
            _allAnswers = new List<Answer>();
            
            // Créer la liste de questions : 3 types par verbe
            _questions = new List<(VerbIrregulier, int)>();
            var random = new Random();
            
            foreach (var verb in verbs)
            {
                // Créer une liste de 3 types de questions [0, 1, 2]
                var questionTypes = new List<int> { 0, 1, 2 };
                
                // Mélanger les types de questions
                questionTypes = questionTypes.OrderBy(x => random.Next()).ToList();
                
                // Ajouter chaque type de question pour ce verbe
                foreach (var type in questionTypes)
                {
                    _questions.Add((verb, type));
                }
            }
            
            // Mélanger l'ordre de toutes les questions
            _questions = _questions.OrderBy(x => random.Next()).ToList();
            
            _questionCount = _questions.Count;
            _answered = false;
            
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                ShowResults();
                return;
            }

            var (verb, questionType) = _questions[_currentQuestionIndex];
            _currentVerb = verb;
            _questionType = questionType;
            _answered = false;

            // Afficher la question
            switch (_questionType)
            {
                case 0: // Infinitif -> Past Simple
                    QuestionText.Text = $"Quel est le Past Simple de '{_currentVerb.Infinitif}' ?";
                    break;
                case 1: // Traduction -> Past Simple
                    QuestionText.Text = $"Conjuguez au Past Simple: '{_currentVerb.Traduction}' ?";
                    break;
                case 2: // Past Simple -> Traduction
                    QuestionText.Text = $"Que signifie '{_currentVerb.PastSimple}' ?";
                    break;
            }

            AnswerTextBox.Clear();
            AnswerTextBox.Focus();
            FeedbackPanel.Children.Clear();
            UpdateProgress();

            NextButton.IsEnabled = false;
            SkipButton.IsEnabled = true;
        }

        private void AnswerTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                ValidateAnswer();
                e.Handled = true;
            }
        }

        private void ValidateAnswer()
        {
            if (_answered) return;

            var userAnswer = AnswerTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(userAnswer)) return;

            _answered = true;
            var isCorrect = IsAnswerCorrect(userAnswer);

            // Enregistrer la reponse
            _allAnswers.Add(new Answer(_currentVerb, isCorrect));

            if (isCorrect)
            {
                _correctAnswers++;
                DisplayCorrectFeedback();
            }
            else
            {
                DisplayIncorrectFeedback(userAnswer);
            }

            AnswerTextBox.IsEnabled = false;
            NextButton.IsEnabled = true;
            SkipButton.IsEnabled = false;
        }

        private bool IsAnswerCorrect(string userAnswer)
        {
            return _questionType switch
            {
                0 or 1 => CompareAnswers(userAnswer, _currentVerb.PastSimple),
                2 => CompareAnswers(userAnswer, _currentVerb.Traduction),
                _ => false
            };
        }

        private bool CompareAnswers(string userAnswer, string correctAnswer)
        {
            // Comparaison flexible permettant les variantes (ex: was/were)
            var userParts = userAnswer.Split('/');
            var correctParts = correctAnswer.Split('/');

            return userParts.Any(part => correctParts.Any(correct => correct.Equals(part, StringComparison.OrdinalIgnoreCase)));
        }

        private void DisplayCorrectFeedback()
        {
            var feedbackBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(232, 245, 233)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(15),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 10, 0, 0)
            };

            var stackPanel = new StackPanel();

            var titleText = new TextBlock
            {
                Text = "✓ CORRECT!",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(titleText);

            var detailsPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 5) };
            detailsPanel.Children.Add(new TextBlock
            {
                Text = $"Infinitif: {_currentVerb.Infinitif}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102))
            });
            detailsPanel.Children.Add(new TextBlock
            {
                Text = $"Past Simple: {_currentVerb.PastSimple}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 2, 0, 0)
            });
            detailsPanel.Children.Add(new TextBlock
            {
                Text = $"Traduction: {_currentVerb.Traduction}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 2, 0, 0),
                FontStyle = FontStyles.Italic
            });
            stackPanel.Children.Add(detailsPanel);

            feedbackBorder.Child = stackPanel;
            FeedbackPanel.Children.Add(feedbackBorder);
        }

        private void DisplayIncorrectFeedback(string userAnswer)
        {
            var feedbackBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 243, 224)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(15),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 10, 0, 0)
            };

            var stackPanel = new StackPanel();

            var titleText = new TextBlock
            {
                Text = "✗ INCORRECT!",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(titleText);

            var yourAnswerText = new TextBlock
            {
                Text = $"Votre reponse: '{userAnswer}'",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                Margin = new Thickness(0, 0, 0, 10),
                FontStyle = FontStyles.Italic
            };
            stackPanel.Children.Add(yourAnswerText);

            var detailsPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };
            detailsPanel.Children.Add(new TextBlock
            {
                Text = "Bonne reponse:",
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 0, 0, 5)
            });
            detailsPanel.Children.Add(new TextBlock
            {
                Text = $"Infinitif: {_currentVerb.Infinitif}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102))
            });
            detailsPanel.Children.Add(new TextBlock
            {
                Text = $"Past Simple: {_currentVerb.PastSimple}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 2, 0, 0)
            });
            detailsPanel.Children.Add(new TextBlock
            {
                Text = $"Traduction: {_currentVerb.Traduction}",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 2, 0, 0),
                FontStyle = FontStyles.Italic
            });
            stackPanel.Children.Add(detailsPanel);

            feedbackBorder.Child = stackPanel;
            FeedbackPanel.Children.Add(feedbackBorder);
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

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_answered)
            {
                // Enregistrer comme incorrect
                _allAnswers.Add(new Answer(_currentVerb, false));
                DisplayIncorrectFeedback("(Passé)");
                AnswerTextBox.IsEnabled = false;
                NextButton.IsEnabled = true;
                SkipButton.IsEnabled = false;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestionIndex++;
            LoadNextQuestion();
            AnswerTextBox.IsEnabled = true;
        }

        private void ShowResults()
        {
            ResultsWindow resultsWindow = new ResultsWindow(_allAnswers);
            resultsWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
