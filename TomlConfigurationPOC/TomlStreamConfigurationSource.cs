using Microsoft.Extensions.Configuration;

namespace TomlConfigurationPOC;

public class TomlStreamConfigurationSource : StreamConfigurationSource
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
        => new TomlStreamConfigurationProvider(this);
}