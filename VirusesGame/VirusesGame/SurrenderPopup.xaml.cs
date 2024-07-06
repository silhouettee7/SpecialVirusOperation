using CommunityToolkit.Maui.Views;
namespace VirusesGame;

public partial class SurrenderPopup : Popup
{
	public SurrenderPopup()
	{
        InitializeComponent();
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