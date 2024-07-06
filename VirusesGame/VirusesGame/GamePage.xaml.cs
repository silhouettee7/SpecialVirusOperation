using Microsoft.Maui.Devices.Sensors;
using System.Xml.Serialization;
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
    private async void BuildBoardButtons()
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
                    boardButtons[i, j].Clicked += OnImageButtonClicked!;
                    Dispatcher.DispatchAsync(() =>
                        boardButtons[i, j].Source = LoadImages(i,j));
                });
            }
        }
    }

    private async void InitializeBoardButtons()
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
                boardButtons[i, j].AutomationId = $"{i},{j}";
                boardButtons[i, j].BorderColor = Colors.Black;
                boardButtons[i, j].BorderWidth = 1;
                BoardGrid.Add(boardButtons[i, j], i, j);
            }
        }

        await Task.Delay(100);
    }

    private void InitializePlayers()
    {
        var player1 = new Player(State.Cross, State.FilledZero, "КРАСНЫЙ");
        var player2 = new Player(State.Zero, State.СircledСross,"ЗЕЛЕНЫЙ");
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
    private async void OnImageButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button != null && leadingPlayer.CountMoves < 3)
        {
            var loctionArr = FindLocationImageButton(button);
            (int x, int y) location = (loctionArr[0], loctionArr[1]);
            var visited = new bool[10, 10];
            if (board[location.x, location.y].State == State.Empty 
                && leadingPlayer.CheckIsCellAvailable(board,location.x,location.y,visited))
            {
                leadingPlayer.Multiply(board, location.x, location.y);
                leadingPlayer.AllLivingCells.Add((location.x,location.y));
                SetActiveStarsImages();
                button.Source = leadingPlayer.Symbols.nativeSymbol == State.Zero ? ImageSource.FromFile("circle.png")
                    : ImageSource.FromFile("cross.png");
            }
            if (board[location.x, location.y].State == secondPlayer.Symbols.nativeSymbol
                && leadingPlayer.CheckIsCellAvailable(board, location.x, location.y, visited))
            {
                leadingPlayer.Kill(board, location.x, location.y);
                secondPlayer.AllLivingCells.Remove((location.x, location.y));
                SetActiveStarsImages();
                button.Source = leadingPlayer.Symbols.nativeSymbol == State.Zero ? ImageSource.FromFile("cross_dead.png")
                    : ImageSource.FromFile("circle_dead.png");
            }
            if (secondPlayer.AllLivingCells.Count == 0)
            {
                await Task.Delay(500);
                await Navigation.PushAsync(new CongratulationPage(leadingPlayer.Name));
            }
        }
    }
    private int[] FindLocationImageButton(ImageButton btn)
    {
        return btn.AutomationId.Split(',')
            .Select(int.Parse)
            .ToArray();
    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if (leadingPlayer.CountMoves == 3)
        {
            ReplacePlayer();
            star1.Source = ImageSource.FromFile("star.png");
            star2.Source = ImageSource.FromFile("star.png");
            star3.Source = ImageSource.FromFile("star.png");
        }
        else
        {
            await DisplayAlert("Оповещение", "Вы сделали меньше 3 ходов. Отмените действие или пропустите ход", "ОК");
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        switch (leadingPlayer.CountMoves)
        {
            case 0:
                await DisplayAlert("Оповещение", "Вы не сделали ни одного хода, поэтому не можете отменить его", "ОК");
                return;
            case 1:
                star1.Source = ImageSource.FromFile("star.png");
                break;
            case 2:
                star2.Source = ImageSource.FromFile("star.png");
                break;
            case 3:
                star3.Source = ImageSource.FromFile("star.png");
                break;
            default:
                throw new ArgumentOutOfRangeException("Превышено количество допустим ходов");
        }
        var loc = leadingPlayer.CancelMove(board);
        boardButtons[loc.x, loc.y].Source = LoadImages(loc.x, loc.y);

    }

    private async void OnSkipButtonClicked(object sender, EventArgs e)
    {
        if (leadingPlayer.CountMoves == 0)
        {
            ReplacePlayer();
        }
        else if (leadingPlayer.CountMoves == 3)
        {
            await DisplayAlert("Оповещение", "Вы сделали 3 хода, нажмите кнопку подвтердить", "ОК");
        }
        else
        {
            await DisplayAlert("Оповещение", "Вы сделали меньше 3 ходов. Отмените действие или сделайте 3 хода", "ОК");
        }
    }
    private void ReplacePlayer()
    {
        leadingPlayer.ResetMoves();
        var tempPlayer = leadingPlayer;
        leadingPlayer = secondPlayer;
        secondPlayer = tempPlayer;
        LeadingPlayer.Text = leadingPlayer.Name.ToUpper();
        LeadingPlayer.TextColor = leadingPlayer.Name == "ЗЕЛЕНЫЙ" ? new Color(23, 113, 0): new Color(181,0,0);
    }
    private ImageSource LoadImages(int x, int y)
    {
        return board[x, y].State == State.Empty ? ImageSource.FromFile("cell.png")
            : board[x, y].State == State.Zero ? ImageSource.FromFile("circle.png")
            : ImageSource.FromFile("cross.png");
    }
    private void SetActiveStarsImages()
    {
        switch (leadingPlayer.CountMoves)
        {
            case 1:
                star1.Source = ImageSource.FromFile("star_active.png");
                break;
            case 2:
                star2.Source = ImageSource.FromFile("star_active.png");
                break;
            case 3:
                star3.Source = ImageSource.FromFile("star_active.png");
                break;
            default:
                throw new ArgumentOutOfRangeException("Превышено кол-во ходов или не сделан ни один");
        }
    }
    
}