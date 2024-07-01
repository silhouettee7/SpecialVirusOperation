namespace VirusesGame;

public partial class CongratulationPage : ContentPage
{
	public CongratulationPage(string winPlayer)
	{
		InitializeComponent();
        CongratulationLabel.Text = winPlayer.ToUpper();
	}

    private async void OnMainMenuButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private async void OnNewGameButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage());
    }
}