using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Tomlet;
using Tomlet.Models;

namespace TomlConfigurationPOC;

internal class TomlConfigurationFileParser
{
    private TomlConfigurationFileParser() { }

    private readonly Dictionary<string, string?> _data = new(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<string> _paths = new();

    public static IDictionary<string, string?> Parse(Stream input) => new TomlConfigurationFileParser().ParseStream(input);

    private IDictionary<string, string?> ParseStream(Stream input)
    {
        using var streamReader = new StreamReader(input);
        var parser = new TomlParser();
        var doc = parser.Parse(streamReader.ReadToEnd());

        VisitTable(doc);

        return _data;
    }

    private void VisitTable(TomlTable table)
    {
        var isEmpty = true;

        foreach (var property in table.Entries)
        {
            isEmpty = false;
            EnterContext(property.Key);
            VisitValue(property.Value);
            ExitContext();
        }

        if (isEmpty && _paths.Count > 0)
        {
            _data[_paths.Peek()] = null;
        }
    }

    private void VisitValue(TomlValue value)
    {
        Debug.Assert(_paths.Count > 0);

        switch (value)
        {
            case TomlTable tbl:
                VisitTable(tbl);
                break;
            
            case TomlArray array:
                var index = 0;
                foreach (var element in array.ArrayValues)
                {
                    EnterContext(index.ToString());
                    VisitValue(element);
                    ExitContext();
                    index++;
                }
                break;
            
            case { } tomlValue:
                var key = _paths.Peek();
                if (_data.ContainsKey(key))
                {
                    throw new FormatException("Key is duplicated");
                }
                _data[key] = tomlValue.StringValue;
                break;
            
            default:
                throw new FormatException("Unsupported TOML");
        }

        // var key = _paths.Peek();
        //
        // if (_data.ContainsKey(key))
        // {
        //     throw new FormatException("Key is duplicated");
        // }
        //
        // _data[key] = value.StringValue;
    }

    private void EnterContext(string context) =>
        _paths.Push(_paths.Count > 0 ? _paths.Peek() + ConfigurationPath.KeyDelimiter + context : context);

    private void ExitContext() => _paths.Pop();
}