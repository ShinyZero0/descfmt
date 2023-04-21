internal static class Program
{
    private static void Main(string[] args)
    {
        StreamReader stdin = new(Console.OpenStandardInput());
        string input = stdin.ReadToEnd();
        TypeDef mainType = new(input);
        Console.WriteLine(mainType.ToString());
    }

    public class TypeDef
    {
        public TypeDef(string raw)
        {
            int firstAngle = raw.IndexOf("<");
            int lastAngle = raw.LastIndexOf(">");
            if (firstAngle != -1 && lastAngle != -1)
            {
                string[] typeAndName = raw.Substring(0, firstAngle).Split(':');
                if (typeAndName.Length == 1)
                    _type = typeAndName[0];
                else
                {
                    _name = typeAndName[0].Trim();
                    _type = typeAndName[1].Trim();
                }
                foreach (
                    string entry in raw.Substring(firstAngle + 1, lastAngle - firstAngle - 1)
                        .Split(',')
                )
                {
                    _subTypes.Add(new TypeDef(entry.Trim()));
                }
            }
            else
            {
                string[] typeAndName = raw.Split(':');
                if (typeAndName.Length == 1)
                    _type = typeAndName[0].Trim();
                else
                {
                    _name = typeAndName[0].Trim();
                    _type = typeAndName[1].Trim();
                }
            }
        }

        string? _name;
        string _type;
        List<TypeDef> _subTypes = new();

        public override string ToString()
        {
            List<string> output = new();
            if (_name != null)
                output.Add($"{_name}: {_type}");
            else
                output.Add(_type);
            if (_subTypes.Count > 2)
            {
                output[0] = $"{output[0]}<";
                _subTypes.ForEach(subtype => output.Add($"    {subtype.ToString()}"));
                output.Add(">");
            }
            else if (_subTypes.Count != 0)
                output[output.Count - 1] = String.Concat(
                    output[output.Count - 1],
                    "<",
                    (String.Join(", ", _subTypes)),
                    ">"
                );
            return String.Join('\n', output);
        }
    }
}
