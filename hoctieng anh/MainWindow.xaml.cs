using System;
using System.Windows;
using EnglishFlashcard3D.ViewModels;

namespace EnglishFlashcard3D
{
    public partial class MainWindow : Window
    {
        private static readonly string GeminiApiKey =
            Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            ?? string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void OpenGenerateVocabularyWindow(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GeminiApiKey))
            {
                MessageBox.Show(
                    "Vui lòng cấu hình GEMINI_API_KEY trước khi dùng tính năng sinh từ.",
                    "Cần API Key",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                var generateWindow = new GenerateVocabularyWindow(GeminiApiKey);
                generateWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Không thể mở cửa sổ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            var flashcardVm = viewModel?.CurrentViewModel;

            if (flashcardVm != null)
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.Space:
                        flashcardVm.FlipCard();
                        e.Handled = true;
                        break;
                    case System.Windows.Input.Key.Right:
                        flashcardVm.NextCard();
                        e.Handled = true;
                        break;
                    case System.Windows.Input.Key.Left:
                        flashcardVm.PrevCard();
                        e.Handled = true;
                        break;
                    case System.Windows.Input.Key.P:
                        flashcardVm.SpeakCurrentWord();
                        e.Handled = true;
                        break;
                    case System.Windows.Input.Key.M:
                    case System.Windows.Input.Key.Enter:
                        flashcardVm.MarkMastered();
                        e.Handled = true;
                        break;
                }
            }
        }
    }
}
