using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadianEngine.Drawing;

public record class DrawRectCommand<TSelf>(int Layer, Rectangle Rect, Color Color)
    : DrawCommand<TSelf>(Layer)
    where TSelf : ArcadianGame<TSelf>
{
    public override void Execute()
    {
        var whiteRectangle = new Texture2D(Context.Game.GraphicsDevice, 1, 1);
        whiteRectangle.SetData([Color.White]);

        Context.Game.spriteBatch.Draw(whiteRectangle, Rect, Color);
    }
}
