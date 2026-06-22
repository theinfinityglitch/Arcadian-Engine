using System.Numerics;
using ArcadianEngine.Core;
using ArcadianEngine.Drawing;
using ArcadianEngine.Math;
using Raylib_cs;

namespace ArcadianEngine.Resources;

public sealed class RenderPipeline<TG>(Vector2I virtualSize) : Resource<TG>, IDisposable
    where TG : ArcadianGame<TG>
{
    private readonly SortedDictionary<int, List<DrawCommand<TG>>> _commands = [];
    private readonly Dictionary<int, RenderTexture2D> _layerTextures = [];
    private readonly List<RenderTexture2D> _frameTextures = [];

    [Export]
    public Vector2I VirtualSize = virtualSize;

    private void Draw(DrawCommand<TG> command)
    {
        if (!_commands.ContainsKey(command.Layer))
            _commands[command.Layer] = [];
        _commands[command.Layer].Add(command);
    }

    public void DrawSprite(Texture2D tex, Vector2 pos, int layer = 0, Color? tint = null)
    {
        Draw(new DrawSpriteCommand<TG>(layer, tex, pos, tint ?? Color.White));
    }

    public void Clear(Color color)
    {
        Draw(new DrawClearCommand<TG>(color));
    }

    public void DrawRect(Rectangle rect, Color color, int layer = 0)
    {
        Draw(new DrawRectCommand<TG>(layer, rect, color));
    }

    public void DrawRectangle(
        Rectangle rect,
        Vector2 origin,
        float rotation,
        Color color,
        int layer = 0
    )
    {
        Draw(new DrawRectangleCommand<TG>(layer, rect, origin, rotation, color));
    }

    public void DrawRing(
        Vector2 center,
        float innerRadius,
        float outerRadius,
        float startAngle,
        float endAngle,
        int segments,
        Color color,
        int layer = 0
    )
    {
        Draw(
            new DrawRingCommand<TG>(
                center,
                innerRadius,
                outerRadius,
                startAngle,
                endAngle,
                segments,
                color,
                layer
            )
        );
    }

    public void DrawCircleSector(
        Vector2 center,
        float radius,
        float startAngle,
        float endAngle,
        int segments,
        Color color,
        int layer = 0
    )
    {
        Draw(
            new DrawCircleSectorCommand<TG>(
                center,
                radius,
                startAngle,
                endAngle,
                segments,
                color,
                layer
            )
        );
    }

    public void DrawTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color, int layer = 0)
    {
        Draw(new DrawTriangleCommand<TG>(v1, v2, v3, color, layer));
    }

    private RenderTexture2D GetOrCreateLayerTexture(int layer)
    {
        if (_layerTextures.TryGetValue(layer, out var texture))
            return texture;

        var newTexture = Raylib.LoadRenderTexture(VirtualSize.X, VirtualSize.Y);
        _layerTextures[layer] = newTexture;
        return newTexture;
    }

    public RenderTexture2D Flush()
    {
        // Final composed texture
        var output = Raylib.LoadRenderTexture(VirtualSize.X, VirtualSize.Y);

        Raylib.BeginTextureMode(output);
        Context.Game._game.OnDraw();
        Context.Game.GameStateMachine.Draw();
        Raylib.EndTextureMode();

        foreach (var (layer, commands) in _commands)
        {
            var layerTex = GetOrCreateLayerTexture(layer);

            Raylib.BeginTextureMode(layerTex);
            Raylib.ClearBackground(Color.Blank);

            foreach (var cmd in commands)
            {
                cmd.Context = Context;
                cmd.Execute();
            }

            Raylib.EndTextureMode();

            // // Compose layer onto output
            // // (apply per-layer shader here if needed)
            Raylib.BeginTextureMode(output);
            Raylib.DrawTextureRec(
                layerTex.Texture,
                new Rectangle(0, 0, VirtualSize.X, -VirtualSize.Y), // flip Y
                Vector2.Zero,
                Color.White
            );
            Raylib.EndTextureMode();
        }

        _commands.Clear();

        return output;
    }

    public void PresentToScreen(RenderTexture2D frame)
    {
        _frameTextures.Add(frame);

        var screenW = Raylib.GetRenderWidth();
        var screenH = Raylib.GetRenderHeight();

        var scale = System.Math.Min((float)screenW / VirtualSize.X, (float)screenH / VirtualSize.Y);

        var destW = (int)(VirtualSize.X * scale);
        var destH = (int)(VirtualSize.Y * scale);
        var offsetX = (screenW - destW) / 2;
        var offsetY = (screenH - destH) / 2;

        Raylib.ClearBackground(Color.Black); // letterbox color

        Raylib.DrawTexturePro(
            frame.Texture,
            new Rectangle(0, 0, VirtualSize.X, -VirtualSize.Y), // flip Y
            new Rectangle(offsetX, offsetY, destW, destH),
            Vector2.Zero,
            0f,
            Color.White
        );
    }

    public void Dispose()
    {
        foreach (var tex in _layerTextures.Values)
            Raylib.UnloadRenderTexture(tex);
        _layerTextures.Clear();
        foreach (var tex in _frameTextures)
            Raylib.UnloadRenderTexture(tex);
        _frameTextures.Clear();
    }
}
