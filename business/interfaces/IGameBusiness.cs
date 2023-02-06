using chessAPI.models.game;

namespace chessAPI.business.interfaces;

public interface IGameBusiness<TI> 
    where TI : struct, IEquatable<TI>
{
    Task<clsGame<TI>> addGame(clsNewGame<TI> newGame);

    Task<clsGame<TI>> startGame(TI whiteTeam,TI defaultTeam);
    Task<clsGame<TI>> getGame(TI id);

    Task<clsGame<TI>> updateGame(clsGame<TI> updatedGame);

    Task<clsGame<TI>> addSecondTeam(TI gameId, TI playerId);

}