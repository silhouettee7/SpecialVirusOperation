
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    public class Player : IPlay
    {
        private Stack<Tuple<int, int>> moves;
        private static ValueTuple<int, int>[] nearbyCellsCoords = new ValueTuple<int, int>[] { (1, 0), (0, 1), (0, -1), (-1, 0)};
        public int InjectionsLeft { get; private set; } = 3;
        public int AllLivingCellsCount { get; private set; } = 1;
        public (State nativeSymbol, State capturedSymbol) Symbols { get; }
        public string Name { get; }
        public int CountMoves { get; private set; }
        public bool IsExpansionDone { get; set; }
        public Player(State nativeSymbol, State capturedSymbol, string name)
        {
            Symbols = (nativeSymbol,capturedSymbol);
            Name = name;
            moves = new Stack<Tuple<int, int>>();
        }
        public void UseInjection(Board board, int x, int y)
        {
            DecreaseNumberOfInjections();
            foreach (var nearbyCoord in nearbyCellsCoords)
            {
                if (x + nearbyCoord.Item1 < 0
                    || y + nearbyCoord.Item2 < 0
                        || x + nearbyCoord.Item1 > 9
                            || y + nearbyCoord.Item2 > 9)
                    continue;
                if (board[x, y].State == board[x + nearbyCoord.Item1, y + nearbyCoord.Item2].State)
                {
                    board[x + nearbyCoord.Item1, y + nearbyCoord.Item2].State = State.Empty;
                    board[x + nearbyCoord.Item1, y + nearbyCoord.Item2].PreviousState = State.Empty;
                }
            }
            board[x, y].State = State.Empty;
            board[x, y].PreviousState = State.Empty;
        }
        public (int x, int y) CancelMove(Board board)
        {
            if (moves.Count > 0)
            {
                var index = moves.Pop();
                CountMoves--;
                board[index.Item1, index.Item2].State = board[index.Item1, index.Item2].PreviousState;
                return (index.Item1, index.Item2);
            }
            else
            {
                throw new IndexOutOfRangeException(nameof(moves));
            }
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
            if (x < 0 || y < 0 || x >= board.xCurrLength || y >= board.yCurrLength
                || visited[x, y]) return false;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0) continue;
                    int xNeighbor = x + i;
                    int yNeighbor = y + j;
                    if (xNeighbor < 0 || yNeighbor < 0
                        || xNeighbor >= board.xCurrLength || yNeighbor >= board.yCurrLength) continue;
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
                    if (xNeighbor < 0 || yNeighbor < 0
                        || xNeighbor >= board.xCurrLength || yNeighbor >= board.yCurrLength) continue;
                    var neighbor = board[xNeighbor, yNeighbor];
                    if (neighbor.State == Symbols.capturedSymbol
                        && CheckIsCellAvailable(board, xNeighbor, yNeighbor, visited)) return true;
                }
            }
            return false;
        }

        public void ResetMoves()
        {
            CountMoves = 0;
            moves.Clear();
        }

        public void DecreaseNumberOfInjections()
        {
            InjectionsLeft--;
        }

        public void IncrementLivingCells()
        {
            AllLivingCellsCount++;
        }

        public void DecrementLivingCells()
        {
            AllLivingCellsCount--;
        }
    }
}
