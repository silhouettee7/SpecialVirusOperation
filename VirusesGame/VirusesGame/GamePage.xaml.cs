using VirusesGame.Classes;
using VirusesGame.Enums;
namespace VirusesGame;

public partial class GamePage : ContentPage
{
    private Board board;
    private ImageButton[,] boardButtons;
    private Player leadingPlayer;
    private Player secondPlayer;
    
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
                    boardButtons[i, j].Clicked += OnImageButtonClicked;
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
        var player1 = new Player(State.Cross, "КРАСНЫЙ");
        var player2 = new Player(State.Zero, "ЗЕЛЕНЫЙ");
        leadingPlayer = player1;
        secondPlayer = player2;
    }

    private async void OnGiveUpButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтвердить действие", "Вы уверены что хотите сдаться?", "Да", "Нет");
        if (result)
        {
            await Navigation.PushAsync(new CongratulationPage(secondPlayer.Name));
        }
    }
    private void OnImageButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button != null)
        {
            var location = FindLocationImageButton(button);
            if (board[location.x, location.y].State == State.Empty)
            {
                leadingPlayer.Multiply(board, location.x, location.y);
                button.Source = leadingPlayer.Symbol == State.Zero ? ImageSource.FromFile("circle.png")
                    : ImageSource.FromFile("cross.png");
            }
            if (board[location.x, location.y].State == secondPlayer.Symbol)
            {
                leadingPlayer.Kill(board, location.x, location.y);
                button.Source = leadingPlayer.Symbol == State.Zero ? ImageSource.FromFile("cross_dead.png")
                    : ImageSource.FromFile("circle_dead.png");
            }
        }
        if (leadingPlayer.IsThreeMovesDone)
        {
            var tempPlayer = leadingPlayer;
            leadingPlayer = secondPlayer;
            secondPlayer = tempPlayer;
            LeadingPlayer.Text = leadingPlayer.Name.ToUpper();
        }
    }
    private (int x, int y) FindLocationImageButton(ImageButton btn)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (boardButtons[i, j].Equals(btn))
                {
                    return (i, j);
                }
            }
        }
        return (-1, -1);
    }

    private void OnConfirmButtonClicked(object sender, EventArgs e)
    {

    }

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {

    }

    private void OnSkipButtonClicked(object sender, EventArgs e)
    {

    }
}