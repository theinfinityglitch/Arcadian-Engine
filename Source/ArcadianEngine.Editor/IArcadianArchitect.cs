namespace ArcadianEngine.Editor;

public class ArcadianArchitect<TSelf, TGame> : ArcadianGame<TGame>
    where TSelf : ArcadianArchitect<TSelf, TGame>
    where TGame : ArcadianGame<TGame> { }

