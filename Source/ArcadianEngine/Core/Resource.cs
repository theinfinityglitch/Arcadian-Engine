namespace ArcadianEngine.Core;

public class Resource<TG>
    where TG : ArcadianGame<TG>
{
#pragma warning disable CS8618
    public GameContext<TG> Context;
#pragma warning restore CS8618

    public virtual void OnContextSet() { }
}
