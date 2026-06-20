namespace ArcadianEngine.Core;

public class Resource<TG>
    where TG : ArcadianGame<TG>
{
    public GameContext<TG> Context = null!;

    public virtual void OnContextSet() { }
}
