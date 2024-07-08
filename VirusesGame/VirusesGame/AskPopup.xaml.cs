using CommunityToolkit.Maui.Views;
namespace VirusesGame;

public partial class AskPopup : Popup
{
	public AskPopup(string message)
	{
        InitializeComponent();
        MessageLabel.Text = message;
    }

    private async void OnYesButtonClicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(true, cts.Token);
    }

    private async void OnNoButtonClicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(false, cts.Token);
    }
}