namespace VirusesGame;

public partial class CongratulationPage : ContentPage
{
	public CongratulationPage(string winPlayer)
	{
		InitializeComponent();
        CongratulationLabel.Text = winPlayer.ToUpper();
        CongratulationLabel.TextColor = winPlayer == "гекемши" ? new Color(23, 113, 0) : new Color(181, 0, 0);
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