using VirusesGame.Classes;
using VirusesGame.Enums;
namespace VirusesGame;

public partial class GamePage : ContentPage
{
    private Board board;
    private ImageButton[,] boardButtons;
    private Player player1;
    private Player player2;
    private Player leadingPlayer;
    
    public GamePage()
    {
        InitializeComponent();
        BuildBoardButtons();
        InitializePlayers();
    }
    public async void BuildBoardButtons()
    {
        boardButtons = new ImageButton[10, 10];
        InitializeBoardButtons();
        board = new Board();
        board.Initialize();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {               
                await Task.Run(() =>
                {
                    Dispatcher.DispatchAsync(() =>
                        boardButtons[i, j].Source = board[i, j].State == State.Empty ? ImageSource.FromFile("cell.png")
                        : board[i, j].State == State.Zero ? ImageSource.FromFile("circle.png") :
                        ImageSource.FromFile("cross.png"));
                });
            }
        }
    }

    public async void InitializeBoardButtons()
    {
        for (int i = 0; i < 10; i++)
        {
            BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
        }

        for (int j = 0; j < 10; j++)
        {
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                boardButtons[i, j] = new ImageButton();
                boardButtons[i, j].BorderColor = Colors.Black;
                boardButtons[i, j].BorderWidth = 1;
                BoardGrid.Add(boardButtons[i, j], i, j);
            }
        }

        await Task.Delay(100);
    }

    public void InitializePlayers()
    {
        player1 = new Player(State.Cross, "КРАСНЫЙ");
        player2 = new Player(State.Zero, "ЗЕЛЕНЫЙ");
        leadingPlayer = player1;
    }

    private async void OnGiveUpButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтвердить действие", "Вы уверены что хотите сдаться?", "Да", "Нет");
        if (result)
        {
            await Navigation.PushAsync(new CongratulationPage(leadingPlayer.Name));
        }
    }
}