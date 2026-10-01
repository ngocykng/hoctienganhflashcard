using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using EnglishFlashcard3D.ViewModels;

namespace EnglishFlashcard3D.Views.Controls
{
    public partial class Card3DView : UserControl
    {
        public Card3DView()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (DataContext is Flashcard3DViewModel vm)
                {
                    vm.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(Flashcard3DViewModel.IsFlipped))
                        {
                            AnimateFlip(vm.IsFlipped);
                        }
                    };
                }
            };
        }

        private void OnCardClicked(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is Flashcard3DViewModel vm)
            {
                vm.FlipCard();
            }
        }

        private void AnimateFlip(bool isFlipped)
        {
            var storyboardName = isFlipped ? "FlipToBackStoryboard" : "FlipToFrontStoryboard";
            if (FindResource(storyboardName) is Storyboard sb)
            {
                sb.Begin();
            }
        }
    }
}
