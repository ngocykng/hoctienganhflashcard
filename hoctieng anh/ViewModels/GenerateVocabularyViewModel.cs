using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EnglishFlashcard3D.Models;
using EnglishFlashcard3D.Services;

namespace EnglishFlashcard3D.ViewModels
{
    public class GenerateVocabularyViewModel : BaseViewModel
    {
        private readonly GeminiService _geminiService;
        private string _topic = string.Empty;
        private int _cardCount = 5;
        private string _selectedLevel = "B1";
        private bool _isGenerating = false;
        private string _statusMessage = string.Empty;
        private ObservableCollection<EnglishFlashcard> _generatedCards;

        private static readonly string[] Levels = { "A1", "A2", "B1", "B2", "C1", "C2", "IELTS" };

        public GenerateVocabularyViewModel(string geminiApiKey)
        {
            _geminiService = new GeminiService(geminiApiKey);
            _generatedCards = new ObservableCollection<EnglishFlashcard>();

            GenerateCommand = new RelayCommand(_ => GenerateVocabulary(), _ => !IsGenerating && !string.IsNullOrWhiteSpace(Topic));
            ClearCommand = new RelayCommand(_ => Clear());
        }

        public string Topic
        {
            get => _topic;
            set => SetField(ref _topic, value);
        }

        public int CardCount
        {
            get => _cardCount;
            set => SetField(ref _cardCount, Math.Max(1, Math.Min(20, value)));
        }

        public string SelectedLevel
        {
            get => _selectedLevel;
            set => SetField(ref _selectedLevel, value);
        }

        public bool IsGenerating
        {
            get => _isGenerating;
            set => SetField(ref _isGenerating, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public ObservableCollection<EnglishFlashcard> GeneratedCards
        {
            get => _generatedCards;
            set => SetField(ref _generatedCards, value);
        }

        public string[] LevelOptions => Levels;

        public ICommand GenerateCommand { get; }
        public ICommand ClearCommand { get; }

        private async void GenerateVocabulary()
        {
            try
            {
                IsGenerating = true;
                StatusMessage = $"🔄 Đang sinh từ vựng cho chủ đề '{Topic}'...";

                var cards = await _geminiService.GenerateVocabularyAsync(Topic, CardCount, SelectedLevel);

                if (cards.Count == 0)
                {
                    StatusMessage = "⚠️ Không thể sinh từ vựng. Vui lòng thử lại.";
                    return;
                }

                GeneratedCards.Clear();
                foreach (var card in cards)
                {
                    GeneratedCards.Add(card);
                }

                StatusMessage = $"✅ Thành công! Đã sinh {cards.Count} từ vựng.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Lỗi: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Generate Error: {ex}");
            }
            finally
            {
                IsGenerating = false;
            }
        }

        private void Clear()
        {
            Topic = string.Empty;
            CardCount = 5;
            SelectedLevel = "B1";
            GeneratedCards.Clear();
            StatusMessage = string.Empty;
        }
    }
}
