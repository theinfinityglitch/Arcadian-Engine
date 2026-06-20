using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record class DrawRectCommand<TG>(int Layer, Rectangle Rect, Color Color)
    : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public override void Execute() => Raylib.DrawRectangleRec(Rect, Color);
}
