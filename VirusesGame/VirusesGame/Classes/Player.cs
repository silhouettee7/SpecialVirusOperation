
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    internal class Player : IPlay
    {
        private Stack<Tuple<int, int>> moves;
        public HashSet<ValueTuple<int, int>> AllLivingCells { get; }
        public (State nativeSymbol, State capturedSymbol) Symbols { get; }
        public string Name { get; }
        public int CountMoves { get; private set; }
        public Player(State nativeSymbol, State capturedSymbol, string name)
        {
            Symbols = (nativeSymbol,capturedSymbol);
            Name = name;
            moves = new Stack<Tuple<int, int>>();
            AllLivingCells = new HashSet<(int, int)>()
            {
                nativeSymbol == State.Zero ? (9,0) : (0,9)
            };
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

        public bool CheckIsCellAvailable(Board board, int x, int y, bool[,] visited)
        {
            if (x < 0 || y < 0 || x > 9 || y > 9 || visited[x,y]) return false;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0) continue;
                    int xNeighbor = x + i;
                    int yNeighbor = y + j;
                    if (xNeighbor < 0 || yNeighbor < 0 || xNeighbor > 9 || yNeighbor > 9) continue;
                    var neighbor = board[xNeighbor, yNeighbor];
                    if (neighbor.State == Symbols.nativeSymbol) return true;
                }
            }
            visited[x, y] = true;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0) continue;
                    int xNeighbor = x + i;
                    int yNeighbor = y + j;
                    if (xNeighbor < 0 || yNeighbor < 0 || xNeighbor > 9 || yNeighbor > 9) continue;
                    var neighbor = board[xNeighbor, yNeighbor];
                    if (neighbor.State == Symbols.capturedSymbol 
                        && CheckIsCellAvailable(board, xNeighbor, yNeighbor, visited)) return true ;
                }
            }
            return false;
        }

        public void ResetMoves()
        {
            CountMoves = 0;
            moves.Clear();
        }
    }
}
