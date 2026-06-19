using ArcadianEngine.Core;
using ArcadianEngine.Drawing;
using ArcadianEngine.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadianEngine.Resources;

public sealed class RenderPipeline<TG>(Vector2I virtualSize) : Resource<TG>, IDisposable
    where TG : ArcadianGame<TG>
{
    private readonly SortedDictionary<int, List<DrawCommand<TG>>> _commands = [];
    private readonly Dictionary<int, RenderTarget2D> _layerTextures = [];
    private readonly List<RenderTarget2D> _frameTextures = [];

    [Export]
    public Vector2I VirtualSize = virtualSize;

    private void Draw(DrawCommand<TG> command)
    {
        if (!_commands.ContainsKey(command.layer))
            _commands[command.layer] = [];
        _commands[command.layer].Add(command);
    }

    public void DrawRect(Rectangle rect, Color color, int layer = 0)
    {
        Draw(new DrawRectCommand<TG>(layer, rect, color));
    }

    private RenderTarget2D GetOrCreateLayerTexture(int layer)
    {
        if (_layerTextures.TryGetValue(layer, out var texture))
            return texture;

        var newTexture = new RenderTarget2D(
            Context.Game.GraphicsDevice,
            Context.Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Context.Game.GraphicsDevice.PresentationParameters.BackBufferHeight
        );
        _layerTextures[layer] = newTexture;
        return newTexture;
    }

    public RenderTarget2D Flush()
    {
        // Final composed texture
        var output = new RenderTarget2D(
            Context.Game.GraphicsDevice,
            Context.Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Context.Game.GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        Context.Game.GraphicsDevice.SetRenderTarget(output);
        Context.Game.GraphicsDevice.Clear(Color.Black);
        Context.Game.GraphicsDevice.SetRenderTarget(null);

        foreach (var (layer, commands) in _commands)
        {
            var layerTex = GetOrCreateLayerTexture(layer);

            Context.Game.GraphicsDevice.SetRenderTarget(layerTex);
            Context.Game.GraphicsDevice.Clear(Color.Transparent);

            Context.Game.spriteBatch.Begin();
            foreach (var cmd in commands)
            {
                cmd.Context = Context;
                cmd.Execute();
            }
            Context.Game.spriteBatch.End();

            Context.Game.GraphicsDevice.SetRenderTarget(null);

            // // Compose layer onto output
            // // (apply per-layer shader here if needed)
            Context.Game.GraphicsDevice.SetRenderTarget(output);
            Context.Game.spriteBatch.Begin();
            Context.Game.spriteBatch.Draw(layerTex, Vector2.Zero, null, Color.White);
            Context.Game.spriteBatch.End();
            Context.Game.GraphicsDevice.SetRenderTarget(null);
        }

        _commands.Clear();

        return output;
    }

    public void PresentToScreen(RenderTarget2D frame)
    {
        _frameTextures.Add(frame);

        var screenW = Context.Game.Window.ClientBounds.Width;
        var screenH = Context.Game.Window.ClientBounds.Height;

        var scale = System.Math.Min((float)screenW / VirtualSize.X, (float)screenH / VirtualSize.Y);

        var destW = (int)(VirtualSize.X * scale);
        var destH = (int)(VirtualSize.Y * scale);
        var offsetX = (screenW - destW) / 2;
        var offsetY = (screenH - destH) / 2;

        Context.Game.GraphicsDevice.Clear(Color.Black); // letterbox color

        Context.Game.spriteBatch.Begin();
        Context.Game.spriteBatch.Draw(
            frame,
            new Rectangle(offsetX, offsetY, destW, destH), // destination: centered on screen with letterboxing
            new Rectangle(0, 0, VirtualSize.X, VirtualSize.Y),
            Color.White
        );
        Context.Game.spriteBatch.End();
    }

    public void Dispose()
    {
        foreach (var tex in _layerTextures.Values)
            tex.Dispose();
        _layerTextures.Clear();
        foreach (var tex in _frameTextures)
            tex.Dispose();
        _frameTextures.Clear();
    }
}
