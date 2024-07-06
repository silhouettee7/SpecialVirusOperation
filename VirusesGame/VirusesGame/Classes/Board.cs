using System.Runtime.CompilerServices;
using VirusesGame.Enums;

namespace VirusesGame.Classes
{
    public class Board
    {
        private Cell[,]? _board;
        public void Initialize()
        {
            _board = new Cell[10,10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _board[i, j] = new Cell();
                    _board[i,j].State = State.Empty;
                }
            }
            _board[0,9].State = State.Cross;
            _board[9,0].State = State.Zero;
        }
        public Cell this[int i, int j]
        {
            get => _board[i, j];
            set => _board[i, j] = value;
        }
    }
}
