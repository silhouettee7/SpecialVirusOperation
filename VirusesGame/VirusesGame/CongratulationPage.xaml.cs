namespace VirusesGame;

public partial class CongratulationPage : ContentPage
{
	public CongratulationPage(string winPlayer, bool tieFlag = false)
	{
		InitializeComponent();
        if (tieFlag)
        {
            CongratulationLabel.Text = "�����";
            CongratulationLabel.TextColor = Colors.BlueViolet;
            ResultLabel.Text = "��� ����������";
        }
        else
        {
            CongratulationLabel.Text = winPlayer.ToUpper();
            CongratulationLabel.TextColor = winPlayer == "��˨���" ? new Color(23, 113, 0) : new Color(181, 0, 0);
        }

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