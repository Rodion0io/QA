using System.Text.RegularExpressions;
using todo.enums;
using todo.Service.Interface;


namespace todo.Service;

public class MacrosService : IMacrosService
{
    public bool CheckMacrosPriority(string title, string pattern)
    {
        Regex regex = new Regex(pattern);
        var value = regex.Matches(title);
        if (value.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckMacrosDate(string title, string pattern)
    {
        Regex regex = new Regex(pattern);
        var value = regex.Match(title);
        if (value.Value != null && value.Value != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public Priority GetPriority(string title, string pattern)
    {
        var match = Regex.Match(title, pattern);
    
        if (!match.Success)
        {
            return (Priority)(-1);
        }

        string priorityStr = match.Value.TrimStart('!');
        if (Enum.TryParse(priorityStr, ignoreCase: true, out Priority priority))
        {
            return priority - 1;
        }

        return (Priority)(-1);
    }

    public DateTime GetDate(string title, string pattern)
    {
        Regex regex = new Regex(pattern);
        
        var value = regex.Match(title);
        
        string dateStr = value.Value.Replace("!before", "");
        
        DateTime date = DateTime.Parse(dateStr);
        
        // DateTime modifyDate = DateTime.SpecifyKind(date.ToUniversalTime(), DateTimeKind.Utc);
        
        //Выдала гпт
        DateTime modifyDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        
        return modifyDate;
    }

    public string deleteMacros(string title, string pattern)
    {
        string newTitle = Regex.Replace(title, pattern, "");
        
        return newTitle;
    }
}