using System.Numerics;
using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record class DrawRectangleCommand<TG>(
    int Layer,
    Rectangle Rect,
    Vector2 Origin,
    float Rotation,
    Color Color
) : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public override void Execute() => Raylib.DrawRectanglePro(Rect, Origin, Rotation, Color);
}
