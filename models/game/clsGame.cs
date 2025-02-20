namespace chessAPI.models.game;

public sealed class clsGame<TI>
    where TI : struct, IEquatable<TI>
{
    public clsGame(TI id, TI whites, TI blacks, bool turn, TI winner)
    {
        this.id = id;
        this.whites = whites;
        this.blacks= blacks;
        this.turn = turn;
        this.winner = winner;
    }

    public TI id { get; set; }
    public TI whites { get; set; }

    public TI blacks { get; set; }

    public bool turn { get; set; }

    public TI winner { get; set; }


}