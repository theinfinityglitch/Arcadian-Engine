namespace ArcadianEngine.Drawing;

public abstract record DrawCommand<TG>(int Layer)
    where TG : ArcadianGame<TG>
{
    public GameContext<TG> Context = null!;

    public abstract void Execute();
}
