using System.Numerics;
using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record class DrawRingCommand<TG>(
    int Layer,
    Vector2 Center,
    float InnerRadius,
    float OuterRadius,
    float StartAngle,
    float EndAngle,
    int Segments,
    Color Color
) : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public override void Execute() =>
        Raylib.DrawRing(Center, InnerRadius, OuterRadius, StartAngle, EndAngle, Segments, Color);
}
