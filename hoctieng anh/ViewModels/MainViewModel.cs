using System.Collections.ObjectModel;
using System.Windows.Input;
using EnglishFlashcard3D.Models;
using EnglishFlashcard3D.Services;

namespace EnglishFlashcard3D.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DataStorageService _dataStorageService;
        private ObservableCollection<TopicDeck> _decks;
        private Flashcard3DViewModel? _currentViewModel;

        public MainViewModel()
        {
            _dataStorageService = new DataStorageService();
            _decks = new ObservableCollection<TopicDeck>();

            StartStudyCommand = new RelayCommand(param => StartStudy(param as TopicDeck));
            LoadDecksCommand = new RelayCommand(_ => LoadDecks());
        }

        public ObservableCollection<TopicDeck> Decks
        {
            get => _decks;
            set => SetField(ref _decks, value);
        }

        public Flashcard3DViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetField(ref _currentViewModel, value);
        }

        public ICommand StartStudyCommand { get; }
        public ICommand LoadDecksCommand { get; }

        private void LoadDecks()
        {
            var decks = _dataStorageService.LoadDecksAsync().Result;
            Decks.Clear();
            foreach (var deck in decks)
            {
                Decks.Add(deck);
            }
        }

        private void StartStudy(TopicDeck? deck)
        {
            if (deck?.Cards.Count > 0)
            {
                CurrentViewModel = new Flashcard3DViewModel(deck.Cards);
            }
        }
    }
}
