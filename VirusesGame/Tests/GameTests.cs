using VirusesGame;
using VirusesGame.Classes;
using VirusesGame.Enums;
namespace Tests
{
    public class GameTests
    {
        [Fact]
        public void Created_Board_Should_Created_Correctly()
        {
            var board = new Board();
            board.Initialize();
            var flag = true;
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if ((i == 0 && j == 9 && board[i, j].State != State.Cross)
                        || (i == 9 && j == 0 && board[i, j].State != State.Zero)
                        || (board[i, j].State != State.Empty))
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag) { break; }
            }
            Assert.True(flag);
        }
    }
}