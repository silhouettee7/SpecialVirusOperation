
using VirusesGame.Interfaces;
using VirusesGame.Enums;
namespace VirusesGame.Classes
{
    internal class Player : IPlay
    {
        private State _symbol;
        public int CountMoves { get; }
        public string Name { get; }
        public Player(State symbol, string name)
        {
            _symbol = symbol;
            Name = name;
        }
        public void CancelMove()
        {
            throw new NotImplementedException();
        }

        public void Kill(Board board, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Multiply(Board board, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SkipMove()
        {
            throw new NotImplementedException();
        }
    }
}
