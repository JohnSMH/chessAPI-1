namespace chessAPI.models.game;

public sealed class clsNewGame
{
    public clsNewGame(int whites, int blacks)
    {
        this.whites = whites;
        this.blacks = blacks;
        this.turn = true;
        this.winner = 0;
    }

    public int whites { get; set; }

    public int blacks { get; set; }

    public bool turn { get; set; }

    public int winner { get; set; }
}
