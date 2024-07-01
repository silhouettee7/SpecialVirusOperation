using VirusesGame.Enums;

namespace VirusesGame.Classes
{
    internal class Board
    {
        private State[,]? _board;
        public void Initialize()
        {
            _board = new State[10,10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _board[i,j] = State.Empty;
                }
            }
            _board[0, 0] = State.Cross;
            _board[9, 9] = State.Zero;
        }
    }
}
