
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    internal class Player : IPlay
    {
        private State _symbol;
        public int CountMoves { get; }
        public string Name { get; }
        private Stack<Func<Tuple<int, int>>> moves;
        public Player(State symbol, string name)
        {
            _symbol = symbol;
            Name = name;
            moves = new Stack<Func<Tuple<int, int>>>();
        }
        public void CancelMove()
        {
            throw new NotImplementedException();
        }

        public void Kill(Board board, int x, int y)
        {
            
        }

        public void Multiply(Board board, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SkipMove()
        {
            throw new NotImplementedException();
        }

        public bool CheckIsCellAvailable(Board board, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
