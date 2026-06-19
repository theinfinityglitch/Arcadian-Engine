namespace ArcadianEngine.Drawing;

public abstract record DrawCommand<TSelf>(int layer)
    where TSelf : ArcadianGame<TSelf>
{
#pragma warning disable CS8618
    public GameContext<TSelf> Context;
#pragma warning restore CS8618

    public abstract void Execute();
}
