using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Layouts;
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
    private bool injectionSelectionMode;
    private static ValueTuple<int, int>[] nearbyCellsCoords = new ValueTuple<int, int>[] {(0, 0), (1, 0), (0, 1), (0, -1), (-1, 0) };

    public GamePage()
    {
        InitializeComponent();
        BuildBoardButtons();
        InitializePlayers();
    }
    private async void BuildBoardButtons()
    {
        boardButtons = new ImageButton[12, 12];
        InitializeBoardButtons();
        board = new Board(12, 12);
        board.Initialize();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                await Task.Run(() =>
                {
                    Dispatcher.DispatchAsync(() =>
                        boardButtons[i, j].Source = LoadImages(i, j));
                });
            }
        }
    }

    private void InitializeBoardButtons()
    {
        for (int i = 0; i < 12; i++)
        {
            BoardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
        }

        for (int j = 0; j < 12; j++)
        {
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
        }

        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                boardButtons[i, j] = new ImageButton();
                boardButtons[i, j].AutomationId = $"{i},{j}";
                boardButtons[i, j].BorderColor = Colors.Black;
                boardButtons[i, j].BorderWidth = 1;
                boardButtons[i, j].Clicked += OnImageButtonClicked!;
                boardButtons[i, j].BackgroundColor = Colors.White;
                if (i < 10 && j < 10) BoardGrid.Add(boardButtons[i, j], j, i);
            }
        }
    }

    private void InitializePlayers()
    {
        var player1 = new Player(State.Cross, State.FilledZero, "КРАСНЫЙ");
        var player2 = new Player(State.Zero, State.СircledСross,"ЗЕЛЁНЫЙ");
        leadingPlayer = player1;
        secondPlayer = player2;
    }

    private async void OnGiveUpButtonClicked(object sender, EventArgs e)
    {
        await DisplaySurrenderPopup();
    }
    private async void OnTieButtonClicked(object sender, EventArgs e)
    {
        await DisplayTiePopup();
    }
    private async void OnImageButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button != null && leadingPlayer.CountMoves < 3)
        {
            var loctionArr = FindLocationImageButton(button);
            (int x, int y) location = (loctionArr[0], loctionArr[1]);
            var visited = new bool[board.xCurrLength, board.yCurrLength];
            if (injectionSelectionMode)
            {
                if (board[location.x, location.y].State == State.Empty
                        || board[location.x, location.y].State == leadingPlayer.Symbols.nativeSymbol
                            || board[location.x, location.y].State == secondPlayer.Symbols.nativeSymbol)
                {
                    leadingPlayer.DecreaseNumberOfInjections();
                    InjectionCount.Text = $": {leadingPlayer.InjectionsLeft}";
                    injectionSelectionMode = !injectionSelectionMode;
                }
                if (board[location.x, location.y].State == leadingPlayer.Symbols.capturedSymbol
                            || board[location.x, location.y].State == secondPlayer.Symbols.capturedSymbol)
                {
                    leadingPlayer.UseInjection(board, location.x, location.y);
                    InjectionCount.Text = $": {leadingPlayer.InjectionsLeft}";
                    foreach (var nearbyCoord in nearbyCellsCoords)
                    {
                        if (location.x + nearbyCoord.Item1 < 0
                            || location.y + nearbyCoord.Item2 < 0
                                || location.x + nearbyCoord.Item1 > 9
                                    || location.y + nearbyCoord.Item2 > 9)
                            continue;
                        if (board[location.x + nearbyCoord.Item1, location.y + nearbyCoord.Item2].State == State.Empty)
                        {
                            boardButtons[location.x + nearbyCoord.Item1, location.y + nearbyCoord.Item2].Source = ImageSource.FromFile("cell.png");
                        }
                    }
                    injectionSelectionMode = !injectionSelectionMode;
                }
            }
            else
            {
                if (board[location.x, location.y].State == State.Empty
                    && leadingPlayer.CheckIsCellAvailable(board, location.x, location.y, visited))
                {
                    leadingPlayer.Multiply(board, location.x, location.y);
                    leadingPlayer.IncrementLivingCells();
                    SetActiveStarsImages();
                    button.Source = leadingPlayer.Symbols.nativeSymbol == State.Zero ? ImageSource.FromFile("circle.png")
                        : ImageSource.FromFile("cross.png");
                }
                if (board[location.x, location.y].State == secondPlayer.Symbols.nativeSymbol
                    && leadingPlayer.CheckIsCellAvailable(board, location.x, location.y, visited))
                {
                    leadingPlayer.Kill(board, location.x, location.y);
                    secondPlayer.DecrementLivingCells();
                    SetActiveStarsImages();
                    button.Source = leadingPlayer.Symbols.nativeSymbol == State.Zero ? ImageSource.FromFile("cross_dead.png")
                        : ImageSource.FromFile("circle_dead.png");
                }
                if (secondPlayer.AllLivingCellsCount == 0)
                {
                    await Task.Delay(500);
                    await Navigation.PushAsync(new CongratulationPage(leadingPlayer.Name));
                }
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
        if (injectionSelectionMode)
        {
            await DisplayNotification("Сначала используйте сыворотку");
        }
        else if (leadingPlayer.CountMoves == 3)
        {
            ReplacePlayer();
            star1.Source = ImageSource.FromFile("star.png");
            star2.Source = ImageSource.FromFile("star.png");
            star3.Source = ImageSource.FromFile("star.png");
        }
        else
        {
            await DisplayNotification("Вы сделали меньше 3 ходов.\nОтмените действие или пропустите ход");
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        if (injectionSelectionMode)
        {
            await DisplayNotification("Сначала используйте сыворотку");
        }
        else
        {
            switch (leadingPlayer.CountMoves)
            {
                case 0:
                    await DisplayNotification("Вы не сделали ни одного хода,\nпоэтому не можете отменить его");
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
                    throw new ArgumentOutOfRangeException("Превышено количество допустимых ходов");
            }
            var loc = leadingPlayer.CancelMove(board);
            boardButtons[loc.x, loc.y].Source = LoadImages(loc.x, loc.y);
        }
    }

    private async void OnSkipButtonClicked(object sender, EventArgs e)
    {
        if (injectionSelectionMode)
        {
            await DisplayNotification("Сначала используйте сыворотку");
        }
        else if (leadingPlayer.CountMoves == 0)
        {
            ReplacePlayer();
        }
        else if (leadingPlayer.CountMoves == 3)
        {
            await DisplayNotification("Вы сделали 3 хода\nнажмите кнопку подтвердить");
        }
        else
        {
            await DisplayNotification("Вы сделали меньше 3 ходов.\nОтмените действие или сделайте 3 хода");
        }
    }
    private void ReplacePlayer()
    {
        leadingPlayer.ResetMoves();
        var tempPlayer = leadingPlayer;
        leadingPlayer = secondPlayer;
        secondPlayer = tempPlayer;
        LeadingPlayer.Text = leadingPlayer.Name.ToUpper();
        LeadingPlayer.TextColor = leadingPlayer.Name == "ЗЕЛЁНЫЙ" ? new Color(23, 113, 0): new Color(181,0,0);
        InjectionCount.Text = $": {leadingPlayer.InjectionsLeft}";
        ExpansionCount.Text = $": {(leadingPlayer.IsExpansionDone ? 0: 1)}";
    }
    private ImageSource LoadImages(int x, int y)
    {
        return board[x, y].State == State.Empty ? ImageSource.FromFile("cell.png")
            : board[x, y].State == State.Zero ? ImageSource.FromFile("circle.png")
            : board[x, y].State == State.СircledСross ? ImageSource.FromFile("cross_dead.png")
            : board[x, y].State == State.FilledZero ? ImageSource.FromFile("circle_dead.png")
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
                throw new ArgumentOutOfRangeException("Превышено кол-во ходов или не сделан ни один ход");
        }
    }
    
    public async Task DisplayNotification(string message)
    {
        var popup = new NotificationPopup(message);
        await this.ShowPopupAsync(popup);
        await Task.Delay(1000);
    }
    public async Task DisplaySurrenderPopup()
    {
        var popup = new AskPopup("Вы уверены\nчто хотите сдаться?");
        var result = await this.ShowPopupAsync(popup, CancellationToken.None);

        if (result is bool boolResult)
        {
            if (boolResult)
            {
                await Navigation.PushAsync(new CongratulationPage(secondPlayer.Name));
            }
        }
    }
    public async Task DisplayTiePopup()
    {
        var popup = new AskPopup("Вы согласны на ничью");
        var result = await this.ShowPopupAsync(popup, CancellationToken.None);

        if (result is bool boolResult)
        {
            if (boolResult)
            {
                await Navigation.PushAsync(new CongratulationPage("", true));
            }
        }
    }

    private async void OnInjectionButtonClicked(object sender, EventArgs e)
    {
        if (leadingPlayer.CountMoves > 0)
            await DisplayNotification("Сыворотку можно использовать только до начала хода");
        else if (leadingPlayer.InjectionsLeft == 0)
            await DisplayNotification("У вас не осталось сывороток");
        else
        {
            injectionSelectionMode = !injectionSelectionMode;
        }
    }

    private async void OnExpansionButtonClicked(object sender, EventArgs e)
    {
        if (leadingPlayer.IsExpansionDone) return;
        var random = new Random();
        var rowOrColumn = random.Next(0, 2);
        var positionInBoard = random.Next(0, rowOrColumn == 0 ? board.xCurrLength + 1 : board.yCurrLength + 1);
        int xStart = rowOrColumn == 0 ? positionInBoard : 0;
        int yStart = rowOrColumn == 1 ? positionInBoard : 0;
        var list = new State[board.yCurrLength];
        var notification = DisplayNotification("Вы использовали расширение!");
        for (int i = xStart; i < board.xCurrLength + (rowOrColumn == 1 ? 0 : 1); i++)
        {
            var tempPrevState = State.Empty;
            for (int j = yStart; j < board.yCurrLength + (rowOrColumn == 0 ? 0 : 1); j++)
            {
                if (rowOrColumn == 1)
                {
                    var tempCurrState = board[i, j].State;
                    board[i, j].State = tempPrevState;
                    tempPrevState = tempCurrState;
                    if (j == board.yCurrLength) BoardGrid.Add(boardButtons[i, j], j, i);
                }
                else
                {
                    var tempCurrState = board[i, j].State;
                    board[i, j].State = list[j];
                    list[j] = tempCurrState;
                    if (i == board.xCurrLength) BoardGrid.Add(boardButtons[i, j], j, i);
                }

                await Task.Run(() => Dispatcher.DispatchAsync(() =>
                        boardButtons[i, j].Source = LoadImages(i, j)));

            }
        }
        if (rowOrColumn == 0) board.xCurrLength++;
        else board.yCurrLength++;
        leadingPlayer.IsExpansionDone = true;
        ExpansionCount.Text = $": 0";
        await notification;
    }

    

}