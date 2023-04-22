internal static class Program
{
    private static void Main(string[] args)
    {
        StreamReader stdin = new(Console.OpenStandardInput());
        string input = stdin.ReadToEnd();
        TypeDef mainType = new(input);
        mainType.ToStringList().ForEach(Console.WriteLine);
    }
}

public class TypeDef
{
    public TypeDef(string raw)
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
                _subTypes.Add(new TypeDef(subTypeRaw));
                lastCut = i + 1;
            }
        }
        _subTypes.Add(new TypeDef(subTypesRaw.Substring(lastCut, subTypesRaw.Length - lastCut)));
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
        _type = _type!.Trim();
    }

    string? _name;
    string _type;
    List<TypeDef> _subTypes = new();

    public List<string> ToStringList()
    {
        List<string> output = new();

        if (_name != null)
            output.Add($"{_name}: {_type}");
        else
            output.Add(_type);

        if (_subTypes.Count > 2)
        {
            output[0] = $"{output[0]}<";
            _subTypes.ForEach(
                subtype => subtype.ToStringList().ForEach(ststring => output.Add($"│   {ststring}"))
            );
            output.Add(">");
        }
        else if (_subTypes.Count > 0)
        {
            List<string> subTypesStrings = new();
            _subTypes.ForEach(st => subTypesStrings.Add(String.Concat(st.ToStringList())));
            output[output.Count - 1] = String.Concat(
                output[output.Count - 1],
                "<",
                (String.Join(", ", subTypesStrings)),
                ">"
            );
        }

        return output;
    }
}
