using Microsoft.Extensions.Configuration;

namespace TomlConfigurationPOC;

public class TomlConfigurationProvider : FileConfigurationProvider
{
    public TomlConfigurationProvider(FileConfigurationSource source) : base(source)
    {
    }

    public override void Load(Stream stream)
    {
        Data = TomlConfigurationFileParser.Parse(stream);
    }
}