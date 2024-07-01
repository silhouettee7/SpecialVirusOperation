using VirusesGame.Classes;

namespace VirusesGame.GamePages;

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
	}
	public void BuildBoardButtons()
	{
		boardButtons = new ImageButton[10,10];
        InitializeBoardButtons();
        board = new Board();
		board.Initialize();

	}
	public async void InitializeBoardButtons()
	{
        for (int i = 0; i < 10; i++)
        {
            Board.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
        }

        for (int j = 0; j < 10; j++)
        {
            Board.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                boardButtons[i, j] = new ImageButton();
                boardButtons[i, j].BorderColor = Colors.Black;
                boardButtons[i, j].BorderWidth = 1;
                Board.Add(boardButtons[i, j], i, j);
            }
        }

        await Task.Delay(100);
    }

}