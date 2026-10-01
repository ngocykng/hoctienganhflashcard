using System.Windows;
using EnglishFlashcard3D.ViewModels;

namespace EnglishFlashcard3D
{
    public partial class GenerateVocabularyWindow : Window
    {
        public GenerateVocabularyWindow(string geminiApiKey)
        {
            InitializeComponent();
            this.DataContext = new GenerateVocabularyViewModel(geminiApiKey);
        }
    }
}
