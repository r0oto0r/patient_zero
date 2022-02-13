public interface Command {
    public string GetName();
    public string GetCommandText();
    public bool Execute();
    public bool Undo();
}
