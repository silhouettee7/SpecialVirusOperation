using CommunityToolkit.Maui.Views;
namespace VirusesGame;

public partial class NotificationPopup : Popup
{
	public NotificationPopup(string message)
	{
		InitializeComponent();
		MessageLabel.Text = message;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		CloseAsync();
    }
}