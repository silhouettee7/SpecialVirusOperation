using System.Runtime.CompilerServices;
using VirusesGame.Enums;

namespace VirusesGame.Classes
{
    public class Board
    {
        private Cell[,] _board;
        public int xCurrLength { get; set; }
        public int yCurrLength { get; set; }
        public Board(int x, int y)
        {
            _board = new Cell[x, y];
            xCurrLength = 10;
            yCurrLength = 10;
        }
        public void Initialize()
        {
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = new Cell();
                    _board[i, j].State = State.Empty;
                }
            }
            _board[9, 0].State = State.Cross;
            _board[0, 9].State = State.Zero;
        }
        public Cell this[int i, int j]
        {
            get => _board[i, j];
            set => _board[i, j] = value;
        }
    }
}
