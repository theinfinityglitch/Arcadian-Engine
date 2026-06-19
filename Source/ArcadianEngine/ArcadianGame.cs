using Microsoft.Xna.Framework;

namespace ArcadianEngine;

/// <summary>
/// This is the main loop of a Arcadian Engine game. This has the callbacks to relevant events in the game.
/// </summary>
public class ArcadianGame<TSelf>
    where TSelf : ArcadianGame<TSelf>
{
#pragma warning disable CS8618
    public GameContext<TSelf> Context;
#pragma warning restore CS8618

    /// <summary>
    /// Called once, when the executable for the game starts and initializes.
    /// </summary>
    public virtual void OnInitialize() { }

    public virtual void OnLoadContent() { }

    /// <summary>
    /// Called after each update.
    /// </summary>
    public virtual void OnUpdate(GameTime time) { }

    /// <summary>
    /// Called before the draw step.
    /// </summary>
    public virtual void OnDraw() { }

    /// <summary>
    /// Called after each draw.
    /// </summary>
    public virtual void OnAfterDraw() { }

    /// <summary>
    /// Called before a scene transition.
    /// </summary>
    public virtual void OnSceneTransition() { }

    /// <summary>
    /// Called once the game exits.
    /// </summary>
    public virtual void OnClose() { }
}
