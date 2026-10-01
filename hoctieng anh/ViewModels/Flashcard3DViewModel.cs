using System.Collections.ObjectModel;
using System.Windows.Input;
using EnglishFlashcard3D.Models;
using EnglishFlashcard3D.Services;

namespace EnglishFlashcard3D.ViewModels
{
    public class Flashcard3DViewModel : BaseViewModel
    {
        private readonly AudioService _audioService;
        private ObservableCollection<EnglishFlashcard> _cards;
        private int _currentIndex = 0;
        private bool _isFlipped = false;

        public Flashcard3DViewModel(List<EnglishFlashcard> cards)
        {
            _audioService = new AudioService();
            _cards = new ObservableCollection<EnglishFlashcard>(cards);

            FlipCommand = new RelayCommand(_ => FlipCard());
            NextCommand = new RelayCommand(_ => NextCard(), _ => CanNext);
            PrevCommand = new RelayCommand(_ => PrevCard(), _ => CanPrev);
            PronounceCommand = new RelayCommand(_ => SpeakCurrentWord());
            MarkMasteredCommand = new RelayCommand(_ => MarkMastered());
        }

        public EnglishFlashcard? CurrentCard => 
            (_cards.Count > 0 && _currentIndex >= 0 && _currentIndex < _cards.Count) 
            ? _cards[_currentIndex] : null;

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (SetField(ref _currentIndex, value))
                {
                    OnPropertyChanged(nameof(CurrentCard));
                    OnPropertyChanged(nameof(ProgressText));
                    IsFlipped = false;
                }
            }
        }

        public bool IsFlipped
        {
            get => _isFlipped;
            set => SetField(ref _isFlipped, value);
        }

        public string ProgressText => $"{_currentIndex + 1} / {_cards.Count}";

        public bool CanNext => _currentIndex < _cards.Count - 1;
        public bool CanPrev => _currentIndex > 0;

        public ICommand FlipCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PrevCommand { get; }
        public ICommand PronounceCommand { get; }
        public ICommand MarkMasteredCommand { get; }

        public void FlipCard() => IsFlipped = !IsFlipped;

        public void NextCard()
        {
            if (CanNext) CurrentIndex++;
        }

        public void PrevCard()
        {
            if (CanPrev) CurrentIndex--;
        }

        public void SpeakCurrentWord()
        {
            if (CurrentCard != null && !string.IsNullOrWhiteSpace(CurrentCard.Word))
            {
                _audioService.SpeakAsync(CurrentCard.Word);
            }
        }

        public void MarkMastered()
        {
            if (CurrentCard != null)
            {
                CurrentCard.IsMastered = true;
                CurrentCard.ReviewCount++;
                CurrentCard.LastReviewedAt = System.DateTime.Now;
                if (CanNext) NextCard();
            }
        }
    }
}
