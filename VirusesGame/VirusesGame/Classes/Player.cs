
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    internal class Player : IPlay
    {
        private Stack<Tuple<int, int>> moves;
        public (State nativeSymbol, State capturedSymbol) Symbols { get; set; }
        public string Name { get; }
        public int CountMoves { get; private set; }
        public Player(State nativeSymbol, State capturedSymbol, string name)
        {
            Symbols = (nativeSymbol,capturedSymbol);
            Name = name;
            moves = new Stack<Tuple<int, int>>();
        }
        public (int x, int y) CancelMove(Board board)
        {
            var index = moves.Pop();
            CountMoves--;
            board[index.Item1, index.Item2].State = board[index.Item1, index.Item2].PreviousState;
            return (index.Item1,index.Item2);
        }

        public void Kill(Board board, int x, int y)
        {
            board[x, y].PreviousState = board[x, y].State;
            board[x, y].State = Symbols.capturedSymbol;
            moves.Push(Tuple.Create(x, y));
            CountMoves++;
        }

        public void Multiply(Board board, int x, int y)
        {
            board[x, y].PreviousState = board[x, y].State;
            board[x, y].State = Symbols.nativeSymbol;
            moves.Push(Tuple.Create(x, y));
            CountMoves++;
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
                        if (neighbor.State == Symbols.nativeSymbol) return true;
                        if (neighbor.State == Symbols.capturedSymbol)
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
        public void Reset()
        {
            CountMoves = 0;
            moves.Clear();
        }
    }
}
