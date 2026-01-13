using RevisionAnglais.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RevisionAnglais
{
    /// <summary>
    /// Logique d'interaction pour AdvancedExerciseWindows.xaml
    /// </summary>
    public partial class AdvancedExerciseWindows : Window
    {
        private List<VerbIrregulier> _verbs;
        private List<VerbIrregulier> _allVerbs;
        private Random _random;
        private int _correctAnswers;
        private int _totalAnswers;
        private VerbIrregulier _currentVerb;
        private List<Answer> _allAnswers;
        private int _questionCount;
        private int _questionType;
        private bool _answered;

        public AdvancedExerciseWindows(List<VerbIrregulier> verbs)
        {
            InitializeComponent();
            _random = new Random();
            _allVerbs = new List<VerbIrregulier>(verbs);
            _verbs = verbs.OrderBy(x => _random.Next()).ToList();
            _correctAnswers = 0;
            _totalAnswers = 0;
            _allAnswers = new List<Answer>();
            _questionCount = _verbs.Count;
            _answered = false;
            _currentVerb = _verbs.Count > 0 ? _verbs[0] : new VerbIrregulier(0, string.Empty, string.Empty, string.Empty);
            LoadNextQuestion();
        }

        private void LoadNextQuestion()
        {
            if (_verbs.Count == 0)
            {
                ShowResults();
                return;
            }

            _currentVerb = _verbs[_random.Next(_verbs.Count)];
            _questionType = _random.Next(3);
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

            _totalAnswers++;
            _verbs.Remove(_currentVerb);

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
                Text = "? CORRECT!",
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
                Text = "? INCORRECT!",
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

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_answered)
            {
                // Enregistrer comme incorrect
                _allAnswers.Add(new Answer(_currentVerb, false));
                _totalAnswers++;
                _verbs.Remove(_currentVerb);

                DisplayIncorrectFeedback("(Passé)");
                AnswerTextBox.IsEnabled = false;
                NextButton.IsEnabled = true;
                SkipButton.IsEnabled = false;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
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
