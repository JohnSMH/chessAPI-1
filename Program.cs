using Autofac;
using Autofac.Extensions.DependencyInjection;
using chessAPI;
using chessAPI.business.interfaces;
using chessAPI.models.player;
using chessAPI.models.game;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Events;

//Serilog logger (https://github.com/serilog/serilog-aspnetcore)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("chessAPI starting");
    var builder = WebApplication.CreateBuilder(args);

    var connectionStrings = new connectionStrings();
    builder.Services.AddOptions();
    builder.Services.Configure<connectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

    // Two-stage initialization (https://github.com/serilog/serilog-aspnetcore)
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom
             .Configuration(context.Configuration)
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).ReadFrom
             .Services(services).Enrich
             .FromLogContext().WriteTo
             .Console());

    // Autofac como inyección de dependencias
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new chessAPI.dependencyInjection<int, int>()));
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(chessAPI.customMiddleware<int>));
    app.MapGet("/", () =>
    {
        return "hola mundo";
    });

    app.MapPost("player", 
    [AllowAnonymous] async(IPlayerBusiness<int> bs, clsNewPlayer newPlayer) => Results.Ok(await bs.addPlayer(newPlayer)));

    app.MapGet("player/{id}",
    [AllowAnonymous] async(IPlayerBusiness<int> bs, int id) => Results.Ok(await bs.getPlayer(id)));

    app.MapPut("player/{id}",
    [AllowAnonymous] async(IPlayerBusiness<int> bs, clsPlayer<int> newPlayer) => Results.Ok(await bs.updatePlayer(newPlayer)));

    app.MapPost("game",
    [AllowAnonymous] async(IGameBusiness<int> bs, clsNewGame<int> newGame) => Results.Ok(await bs.addGame(newGame)));

    app.MapPost("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int whiteTeam) => Results.Ok(await bs.startGame(whiteTeam,1)));

    app.MapGet("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int id) => Results.Ok(await bs.getGame(id)));

    app.MapPut("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, clsGame<int> updatedGame) => Results.Ok(await bs.updateGame(updatedGame)));

    app.MapPut("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int idGame, int blackPlayer) => Results.Ok(await bs.addSecondTeam(idGame,blackPlayer)));

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "chessAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
