internal static class Program
{
    private static void Main(string[] args)
    {
        StreamReader stdin = new(Console.OpenStandardInput());
        string input = stdin.ReadToEnd();
        DescType rootType = new(input);
        Console.WriteLine(rootType.FormatString());
    }
}

public class DescType
{
    public DescType(string raw)
    {
        int firstAngle = raw.IndexOf("<");
        int lastAngle = raw.LastIndexOf(">");
        if (firstAngle == -1 && lastAngle == -1)
        {
            _setTypeAndName(raw.Split(':'));
            return;
        }
        _setTypeAndName(raw.Substring(0, firstAngle).Split(':'));
        int anglesDepth = 0;
        int lastCut = 0;
        string subTypesRaw = raw.Substring(firstAngle + 1, lastAngle - firstAngle - 1);
        for (int i = 0; i < subTypesRaw.Length; i++)
        {
            char c = subTypesRaw[i];
            if (c == '<')
                anglesDepth++;
            else if (c == '>')
                anglesDepth--;
            else if (c == ',' && anglesDepth == 0)
            {
                string subTypeRaw = subTypesRaw.Substring(lastCut, i - lastCut);
                _subTypes.Add(new DescType(subTypeRaw));
                lastCut = i + 1;
            }
        }
        _subTypes.Add(new DescType(subTypesRaw.Substring(lastCut, subTypesRaw.Length - lastCut)));
    }

    void _setTypeAndName(string[] typeAndName)
    {
        if (typeAndName.Length == 1)
            _type = typeAndName[0];
        else
        {
            _name = typeAndName[0];
            _type = typeAndName[1];
        }

        if (_name != null)
            _name = _name.Trim();
        _type = _type.Trim();
    }

    int _getRecursiveCount() =>
        this._subTypes.Select(st => st._getRecursiveCount()).Sum() + this._subTypes.Count;

    string? _name;
    string _type;
    List<DescType> _subTypes = new();

    public string FormatString()
    {
        string firstLine = _name != null ? $"{_name}: {_type}" : _type;

        List<string> output = new();
        if (this._getRecursiveCount() > 3)
        {
            output.Add($"{firstLine}<");
            foreach (DescType subtype in this._subTypes)
            {
                foreach (string ststring in subtype.FormatString().Split('\n'))
                {
                    output.Add($"│   {ststring}");
                }
            }
            output.Add(">");

            return String.Join('\n', output);
        }
        else if (this._getRecursiveCount() > 0)
        {
            List<string> subTypesStrings = new();
            _subTypes.ForEach(
                subtype => subTypesStrings.Add(String.Concat(subtype.FormatString()))
            );
            return String.Concat(firstLine, "<", String.Join(", ", subTypesStrings), ">");
        }

        return firstLine;
    }
}
