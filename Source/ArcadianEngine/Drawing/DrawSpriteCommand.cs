using System.Numerics;
using Raylib_cs;

namespace ArcadianEngine.Drawing;

public record DrawSpriteCommand<TG>(int Layer, Texture2D Texture, Vector2 Position, Color Tint)
    : DrawCommand<TG>(Layer)
    where TG : ArcadianGame<TG>
{
    public override void Execute() => Raylib.DrawTextureV(Texture, Position, Tint);
}
