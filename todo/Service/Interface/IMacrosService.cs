using todo.enums;

namespace todo.Service.Interface;

public interface IMacrosService
{
    public Priority GetPriority(string title, string pattern);

    public bool CheckMacrosPriority(string title, string pattern);

    public bool CheckMacrosDate(string title, string pattern);

    public DateTime GetDate(string title, string pattern);

    public string deleteMacros(string title, string pattern);
}