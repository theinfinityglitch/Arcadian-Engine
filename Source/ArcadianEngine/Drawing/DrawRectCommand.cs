using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadianEngine.Drawing;

public record class DrawRectCommand<TSelf>(int Layer, Rectangle Rect, Color Color)
    : DrawCommand<TSelf>(Layer)
    where TSelf : ArcadianGame<TSelf>
{
    private static Texture2D? _whiteTexture;

    public override void Execute()
    {
        if (_whiteTexture == null || _whiteTexture.IsDisposed)
        {
            _whiteTexture = new Texture2D(Context.Game.GraphicsDevice, 1, 1);
            _whiteTexture.SetData([Color.White]);
        }

        Context.Game.spriteBatch.Draw(_whiteTexture, Rect, Color);
    }
}
