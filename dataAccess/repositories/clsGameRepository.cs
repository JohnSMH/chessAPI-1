using chessAPI.dataAccess.common;
using chessAPI.dataAccess.interfaces;
using chessAPI.dataAccess.models;
using chessAPI.models.game;
using Dapper;

namespace chessAPI.dataAccess.repositores;

public sealed class clsGameRepository<TI, TC> : clsDataAccess<clsGameEntityModel<TI, TC>, TI, TC>, IGameRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    public clsGameRepository(IRelationalContext<TC> rkm,
                               ISQLData queries,
                               ILogger<clsGameRepository<TI, TC>> logger) : base(rkm, queries, logger)
    {
    }

    public async Task<TI> addGame(clsNewGame game)
    {
        var p = new DynamicParameters();
        p.Add("WHITES", game.whites);
        p.Add("BLACKS", game.blacks);
        p.Add("TURN",game.turn);
        return await add<TI>(p).ConfigureAwait(false);
    }

    public Task deleteGame(TI id)
    {
        throw new NotImplementedException();
    }


    public async Task<clsGameEntityModel<TI, TC>> getGame(TI id)
    {
        return await getEntity(id).ConfigureAwait(false);
    }

    public async Task updateGame(clsGame<TI> updatedGame)
    {
        var p = fieldsAsParams(new clsGameEntityModel<TI, TC>() { id = updatedGame.id, whites = updatedGame.whites, blacks = updatedGame.blacks, turn = updatedGame.turn, winner = updatedGame.winner});
        await set(p,null).ConfigureAwait(false);
    }

    protected override DynamicParameters fieldsAsParams(clsGameEntityModel<TI, TC> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var p = new DynamicParameters();
        p.Add("ID", entity.id);
        p.Add("WHITES", entity.whites);
        p.Add("BLACKS", entity.blacks);
        p.Add("TURN", entity.turn);
        p.Add("WINNER", entity.winner);
        return p;
    }

    protected override DynamicParameters keyAsParams(TI key)
    {
        var p = new DynamicParameters();
        p.Add("ID", key);
        return p;
    }

}