namespace VirusesGame.GamePages;

public partial class TutorialPage : ContentPage
{
	public TutorialPage()
	{
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}