using VirusesGame.GamePages;

namespace VirusesGame
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnPlayButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }

        private void OnTutorialButtonClicked(object sender, EventArgs e)
        {

        }

        private void OnExitButtonClicked(object sender, EventArgs e)
        {

        }
    }

}
