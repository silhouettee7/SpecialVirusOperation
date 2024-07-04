
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    internal class Player : IPlay
    {
        private Stack<Tuple<int, int>> moves;
        
        private bool isThreeMovesDone;

        public bool IsThreeMovesDone
        {
            get 
            { 
                if (CountMoves == 3)
                {
                    CountMoves = 0;
                    return true;
                }
                return false;
            }
            private set { isThreeMovesDone = value; }
        }

        public State Symbol { get; }
        public string Name { get; }
        public int CountMoves { get; private set; }
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
            board[x, y].PreviousState = board[x, y].State;
            board[x, y].State = Symbol == State.Cross ? State.FilledZero
                : State.СircledСross;
            moves.Push(Tuple.Create(x, y));
            CountMoves++;
            if (moves.Count == 3)
            {
                moves.Clear();
            }
        }

        public void Multiply(Board board, int x, int y)
        {
            board[x, y].PreviousState = board[x, y].State;
            board[x, y].State = Symbol;
            moves.Push(Tuple.Create(x, y));
            CountMoves++;
            if (moves.Count == 3)
            {
                moves.Clear();
            }
        }

        public void SkipMove()
        {
            throw new NotImplementedException();
        }

        public bool CheckIsCellAvailable(Board board, int x, int y)
        {
            (int currX, int currY) currentCellLoc = (x,y);
            bool succes = true;
            while (currentCellLoc.currX >= 0 && currentCellLoc.currY >= 0
                && currentCellLoc.currX <= 9 && currentCellLoc.currY <= 9)
            {
                succes = false;
                bool flag = false;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (j == 0 && i == 0) continue;
                        int xNeighbor = currentCellLoc.currX + i;
                        int yNeighbor = currentCellLoc.currY + j;
                        if (xNeighbor < 0 || yNeighbor < 0 || xNeighbor > 9 || yNeighbor > 9) continue;
                        var neighbor = board[xNeighbor, yNeighbor];
                        if (neighbor.State == Symbol) return true;
                        if (neighbor.State == (Symbol == State.Cross ? State.FilledZero
                            : State.СircledСross))
                        {
                            succes = true;
                            currentCellLoc = (xNeighbor, yNeighbor);
                            flag = true;
                            break;
                        }
                    }
                    if (flag) break;
                }
                if (!succes) break;
            }

            return succes;
            
        }
    }
}
