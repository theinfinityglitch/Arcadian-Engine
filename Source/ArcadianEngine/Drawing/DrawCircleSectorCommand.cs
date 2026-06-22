using System.Numerics;
using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record DrawCircleSectorCommand<TG>(
    Vector2 Center,
    float Radius,
    float StartAngle,
    float EndAngle,
    int Segments,
    Color Color,
    int Layer = 0
) : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public override void Execute()
    {
        Raylib.DrawCircleSector(Center, Radius, StartAngle, EndAngle, Segments, Color);
    }
}
