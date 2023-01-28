namespace Commitex.Console;

public class DiffProvider
{
    public string GetDiff()
    {
        string diff;
        using (var reader = new StreamReader(System.Console.OpenStandardInput()))
        {
            diff = reader.ReadToEnd();
        }

        return diff;
    }
}