using Microsoft.Extensions.Configuration;

namespace TomlConfigurationPOC;

public class TomlStreamConfigurationProvider : StreamConfigurationProvider
{
    public TomlStreamConfigurationProvider(StreamConfigurationSource source) : base(source) { }

    public override void Load(Stream stream)
    {
        Data = TomlConfigurationFileParser.Parse(stream);
    }
}