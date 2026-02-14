namespace ArcadianEngine.Data;

public class GameDataManager(IArcadianGame? game) : IDisposable
{
    private readonly IArcadianGame? _game = game;
    string? binResourcesDirectory;

    public void Initialize(string resourcesBinPath = "resources")
    {
        binResourcesDirectory = resourcesBinPath;
        Console.WriteLine(Path.Join(binResourcesDirectory, "test.json"));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
