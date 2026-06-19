using ArcadianEngine.Core;
using ArcadianEngine.Math;
using ArcadianEngine.Resources;
using ArcadianEngine.StateMachines;
using ArcadianEngine.Systems;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadianEngine;

/// <summary>
/// This class is the entry point for most games. Handles setting up a window and graphics and runs a game loop
/// </summary>
/// <typeparam name="TG">This is the main loop of an arcadian game. This has the callbacks to relevant events in the game.</typeparam>
public partial class Game<TG> : Game
    where TG : ArcadianGame<TG>
{
    private readonly TG _game;
    private readonly GameContext<TG> _context;
    public GraphicsDeviceManager _graphics;
    public SpriteBatch spriteBatch;
    public Rectangle _windowedBounds;

    // public GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;

    public readonly ResourceContainer<TG> ResourceContainer;
    public readonly EntityStore World = new();
    public readonly LinearStateMachine<TG> GameStateMachine;
    public bool ShouldClose = false;

    // private bool _drawWorldInspector;
    // private bool _drawConsole;

    public Game(TG game)
    {
        _game = game;
        _context = new GameContext<TG>(this);
        ResourceContainer = new(_context);
        _game.Context = _context;
        _graphics = new GraphicsDeviceManager(this);
        GameStateMachine = new LinearStateMachine<TG>("GameStateMachine", _context);
    }

    ~Game()
    {
        ResourceContainer.Dispose();
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        spriteBatch = new SpriteBatch(GraphicsDevice);

        Vector2I windowSize = new(Window.ClientBounds.Width, Window.ClientBounds.Height);
        _context.InsertResource(new MainScheduleOrder<TG>());
        _context.InsertResource(new RenderPipeline<TG>(windowSize));
        // _context.InsertResource(new WorldHierarchyDebug<TG>(_context));
        // _context.InsertResource(new ImGuiConsole());

        _context.InsertSystem<PostUpdate, TransformPropagationSystem>(
            new TransformPropagationSystem()
        );
    }

    protected override void BeginRun()
    {
        base.BeginRun();

        _game.OnInitialize();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _game.OnUpdate(gameTime);

        _context.TryGetResource<MainScheduleOrder<TG>>(out var resource);
        resource?.Run();
    }

    protected override void EndDraw()
    {
        if (!_context.TryGetResource<RenderPipeline<TG>>(out var rp))
            return;

        var frame = rp.Flush();

        rp.PresentToScreen(frame);

        base.EndDraw();
    }
}
