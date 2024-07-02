
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    internal class Player : IPlay
    {
        public bool IsThreeMovesDone { get; private set; }
        public State Symbol { get; }
        public string Name { get; }
        private Stack<Tuple<int, int>> moves;
        public Player(State symbol, string name)
        {
            Symbol = symbol;
            Name = name;
            moves = new Stack<Tuple<int, int>>();
        }
        public bool CancelMove(Board board)
        {
            if (moves.Count == 0) return false;
            var index = moves.Pop();
            board[index.Item1, index.Item2].State = board[index.Item1, index.Item2].PreviousState;
            return true;
        }

        public void Kill(Board board, int x, int y)
        {
            IsThreeMovesDone = false;
            if (CheckIsCellAvailable(board, x, y))
            {
                board[x, y].PreviousState = board[x, y].State;
                board[x, y].State = Symbol == State.Cross ? State.FilledZero
                    :State.СircledСross;
                moves.Push(Tuple.Create(x, y));
                if (moves.Count == 3)
                {
                    moves.Clear();
                    IsThreeMovesDone = true;
                }
            }
        }

        public void Multiply(Board board, int x, int y)
        {
            IsThreeMovesDone = false;
            if (CheckIsCellAvailable(board, x, y))
            {
                board[x, y].PreviousState = board[x, y].State;
                board[x, y].State = Symbol;
                moves.Push(Tuple.Create(x, y));
                if (moves.Count == 3)
                {
                    moves.Clear();
                    IsThreeMovesDone = true;
                }
            } 
        }

        public void SkipMove()
        {
            throw new NotImplementedException();
        }

        public bool CheckIsCellAvailable(Board board, int x, int y)
        {
            return true;
        }
    }
}
