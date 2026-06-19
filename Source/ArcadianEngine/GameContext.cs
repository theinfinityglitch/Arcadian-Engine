using System.Diagnostics.CodeAnalysis;
using ArcadianEngine.Core;
using ArcadianEngine.Resources;
using Friflo.Engine.ECS.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadianEngine;

public class GameContext<TG>(Game<TG> game)
    where TG : ArcadianGame<TG>
{
    public Game<TG> Game { get; private set; } = game;

    public void Quit()
    {
        Game.ShouldClose = true;
    }

    public bool IsBorderlessWindow()
    {
        return Game.Window.IsBorderless;
    }

    public void ToggleBorderlessWindow()
    {
        if (IsBorderlessWindow())
        {
            // Save current windowed bounds so we can restore them later
            Game._windowedBounds = Game.Window.ClientBounds;

            // Apply borderless fullscreen
            Game._graphics.HardwareModeSwitch = false;
            Game._graphics.IsFullScreen = true;
            Game._graphics.PreferredBackBufferWidth = GraphicsAdapter
                .DefaultAdapter
                .CurrentDisplayMode
                .Width;
            Game._graphics.PreferredBackBufferHeight = GraphicsAdapter
                .DefaultAdapter
                .CurrentDisplayMode
                .Height;
            Game.Window.IsBorderless = true;
        }
        else
        {
            // Revert back to windowed
            Game._graphics.IsFullScreen = false;
            Game._graphics.HardwareModeSwitch = true;
            Game.Window.IsBorderless = false;

            Game._graphics.PreferredBackBufferWidth = Game._windowedBounds.Width;
            Game._graphics.PreferredBackBufferHeight = Game._windowedBounds.Height;
        }

        Game._graphics.ApplyChanges();

        // On Windows, borderless window might not snap to (0,0) automatically
        if (IsBorderlessWindow())
        {
            Game.Window.Position = new Point(0, 0);
        }
    }

    public void InsertGameState<T>(T state)
        where T : State<TG>, new()
    {
        Game.GameStateMachine.AddState(state);
    }

    public TSystemType InsertSystem<TSchedule, TSystemType>(TSystemType system)
        where TSchedule : struct, ISchedule
        where TSystemType : BaseSystem
    {
        return GetResource<MainScheduleOrder<TG>>().InsertSystem<TSchedule, TSystemType>(system);
    }

    public void RemoveSystem<T>(BaseSystem system)
        where T : struct, ISchedule
    {
        GetResource<MainScheduleOrder<TG>>().RemoveSystem<T>(system);
    }

    public void InsertResource<TRes>(TRes resource)
        where TRes : Resource<TG> => Game.ResourceContainer.InsertResource(resource);

    public TRes GetResource<TRes>()
        where TRes : Resource<TG> => Game.ResourceContainer.GetResource<TRes>();

    public bool TryGetResource<TRes>([MaybeNullWhen(false)] out TRes resource)
        where TRes : Resource<TG> => Game.ResourceContainer.TryGetResource(out resource);

    public IReadOnlyDictionary<Type, object> GetAllResources() =>
        Game.ResourceContainer.GetAllResources();

    public TRes GetResource<TRes>(Action<TRes> actions)
        where TRes : Resource<TG>
    {
        var resource = GetResource<TRes>();

        actions(resource);

        return resource;
    }
}
