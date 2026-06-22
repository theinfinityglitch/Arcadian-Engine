using System.Numerics;
using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record DrawTriangleCommand<TG>(
    Vector2 V1,
    Vector2 V2,
    Vector2 V3,
    Color Color,
    int Layer = 0
) : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public override void Execute()
    {
        Raylib.DrawTriangle(V1, V2, V3, Color);
    }
}
