using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record DrawClearCommand<TG>(Color Color, int Layer = 0) : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public DrawClearCommand(Color Color)
        : this(Color, 0) { }

    public override void Execute()
    {
        Raylib.ClearBackground(Color);
    }
}
