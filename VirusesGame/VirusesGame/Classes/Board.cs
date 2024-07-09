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
        public Cell this[int i, int j]
        {
            get => _board[i, j];
            set => _board[i, j] = value;
        }
    }
}
