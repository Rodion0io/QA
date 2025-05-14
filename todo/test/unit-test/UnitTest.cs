namespace todo.test.unit_test;

using todo.enums;
using todo.constants;
using Xunit;
using todo.Service;


public class UnitTest
{

    private readonly MacrosService _macrosService;

    public UnitTest()
    {
        _macrosService = new MacrosService();
    }

    
    ///<summary>Проверяет корректность поиска даты в строке</summary>
    ///<param name="sentence">Строка, которая может содержать макрос</param>
    ///<param name="pattern">Шаблон макроса</param>
    ///<param name="expected">Ожидаемое значение</param>
    [Theory]
    [InlineData("tsdgsdsadg!before 10.01.2005", Patterns.dateMacros, true)]
    [InlineData("gsdsadgsdgr !before 12.12.2012", Patterns.dateMacros, true)]
    [InlineData("fsagasdg !before 29-04-2025 sdgasdgsd", Patterns.dateMacros, true)]
    [InlineData("fsagasdg !before 31-12-2999 sdgasdgsd", Patterns.dateMacros, true)]
    [InlineData("fsagasdg !before 01-01-2000 sdgasdgsd", Patterns.dateMacros, true)]
    // [InlineData("fsagasdg !before 30-02-2025 sdgasdgsd", Patterns.dateMacros, true)]
    //Этот тест будет не проходить
    // [InlineData("sdkjfxgb!before32-04-2025", Patterns.dateMacros, true)]
    // [InlineData("", Patterns.dateMacros, true)]
    public void CheckMacrosDate_CorrectValues_ReturnTrue(string sentence, string pattern, bool expected)
    {
        var res = _macrosService.CheckMacrosDate(sentence, pattern);

        Assert.Equal(expected, res);
    }
    
    /// <summary>Проверяет корректность поиска даты в строке(Точнее обработку невалидных данных)</summary>
    ///<param name="sentence">Строка, которая может содержать макрос</param>
    ///<param name="pattern">Шаблон макроса</param>
    ///<param name="expected">Ожидаемое значение</param>
    [Theory]
    [InlineData("test", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before29-04-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before32-12-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before00-12-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before-1-12-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before -1-12-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd! 01-12-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before 40-40-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before 40-2025", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd!before 11-04-2", Patterns.dateMacros, false)]
    [InlineData("soreidfngosjsd before 11-04-2222", Patterns.dateMacros, false)]
    [InlineData("sorei!before 11dfngosjsd -04-2222", Patterns.dateMacros, false)]
    [InlineData("", Patterns.dateMacros, false)]
    public void CheckMacrosDate_InCorrectValues_ReturnFalse(string sentence, string pattern, bool expected)
    {
        var res = _macrosService.CheckMacrosDate(sentence, pattern);

        Assert.Equal(expected, res);
    }
    
    
    /// <summary>Проверяет корректность поиска значение приоритета в строке</summary>
    ///<param name="sentence">Строка, которая может содержать макрос</param>
    ///<param name="pattern">Шаблон макроса</param>
    ///<param name="expected">Ожидаемое значение</param>
    [Theory]
    [InlineData("te!1st", Patterns.priorityMacros, true)]
    [InlineData("!3testdsrhgkjnesr", Patterns.priorityMacros, true)]
    [InlineData("test!4", Patterns.priorityMacros, true)]
    [InlineData("test!2", Patterns.priorityMacros, true)]
    public void CheckMacrosPriority_CorrectValues_ReturnTrue(string sentence, string pattern, bool expected)
    {
        var res = _macrosService.CheckMacrosPriority(sentence, pattern);

        Assert.Equal(expected, res);
    }
    
    /// <summary>Проверяет корректность поиска значение приоритета в строке(Точнее обработку невалидных данных)</summary>
    ///<param name="sentence">Строка, которая может содержать макрос</param>
    ///<param name="pattern">Шаблон макроса</param>
    ///<param name="expected">Ожидаемое значение</param>
    [Theory]
    [InlineData("test", Patterns.priorityMacros, false)]
    [InlineData("test!6", Patterns.priorityMacros, false)]
    [InlineData("test!5", Patterns.priorityMacros, false)]
    [InlineData("test!0", Patterns.priorityMacros, false)]
    [InlineData("", Patterns.priorityMacros, false)]
    [InlineData("-ew687w83q740938q", Patterns.priorityMacros, false)]
    [InlineData("!g", Patterns.priorityMacros, false)]
    [InlineData("вшариешщуоырв!ошщк2", Patterns.priorityMacros, false)]
    [InlineData("вшариешщуоырв!ошщк2", Patterns.priorityMacros, false)]
    public void CheckMacrosPriority_InCorrectValues_ReturnFalse(string sentence, string pattern, bool expected)
    {
        var res = _macrosService.CheckMacrosPriority(sentence, pattern);
        
        Assert.Equal(expected, res);
    }
    
    /// <summary>Вытаскивает значение приоритета из строки</summary>
    ///<param name="sentence">Строка, которая содержит макрос</param>
    ///<param name="pattern">Шаблон макроса</param>
    ///<param name="expected">Ожидаемое значение</param>
    [Theory]
    [InlineData("te!1st", Patterns.priorityMacros, Priority.CRITICAL)]
    [InlineData("!3testdsrhgkjnesr", Patterns.priorityMacros, Priority.MEDIUM)]
    [InlineData("!4testdsrhgkjnesr!6", Patterns.priorityMacros, Priority.LOW)]
    [InlineData("", Patterns.priorityMacros, (Priority)(-1))]
    [InlineData("фвклориаыптыолв!0", Patterns.priorityMacros, (Priority)(-1))]
    [InlineData("фвклориаыптыолв!-11", Patterns.priorityMacros, (Priority)(-1))]
    public void CheckGetPriority_CorrectValues_ReturnTrue(string sentence, string pattern, Priority expected)
    {
        var res = _macrosService.GetPriority(sentence, pattern);
        
        Assert.Equal(expected, res);
    }
    
    /// <summary>Вытаскивает дату из строки</summary>
    ///<param name="sentence">Строка, которая содержит макрос</param>
    ///<param name="pattern">Шаблон макроса</param>
    ///<param name="expected">Ожидаемое значение</param>
    [Theory]
    [InlineData("test!before 10.01.2005", Patterns.dateMacros, "10.01.2005 00:00:00")]
    [InlineData("ejdrknlfgerodsjfgldkn !before 12.12.2012", Patterns.dateMacros, "12.12.2012 00:00:00")]
    [InlineData("skjdfgnls !before 29-04-2025 sjfkgdnl", Patterns.dateMacros, "29-04-2025 00:00:00")]
    [InlineData("skjdfgnls !before 31-12-2999 sjfkgdnl", Patterns.dateMacros, "31.12.2999 00:00:00")]
    [InlineData("skjdfgnls !before 01.01.2000 sjfkgdnl", Patterns.dateMacros, "01.01.2000 00:00:00")]
    [InlineData("skjdfgnls !before 15.05.2309 sjfkgdnl", Patterns.dateMacros, "15.05.2309 00:00:00")]
    [InlineData("skjdfgnls !before 31.12.2999 sjfkgdnl", Patterns.dateMacros, "31.12.2999 00:00:00")]
    // [InlineData("test", Patterns.dateMacros, "2025-05-01 00:00:00")]
    public void CheckGetDate_CorrectValues_ReturnTrue(string sentence, string pattern, string expected)
    {
        
        var res = _macrosService.GetDate(sentence, pattern);

        var newDate = ModifyDate(expected);
        
        Assert.Equal(newDate, res);
    }

    private DateTime ModifyDate(string date)
    {
        return DateTime.SpecifyKind(DateTime.Parse(date), DateTimeKind.Utc);
    }
}