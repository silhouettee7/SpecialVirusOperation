using VirusesGame.Classes;
namespace VirusesGame.Interfaces
{
    internal interface IPlay
    {
        void Kill(Board board,int x, int y);
        void Multiply(Board board, int x, int y);
        (int x, int y) CancelMove(Board board);
    }
}
