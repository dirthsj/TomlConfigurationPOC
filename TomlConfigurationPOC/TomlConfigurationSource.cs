using Microsoft.Extensions.Configuration;

namespace TomlConfigurationPOC;

public class TomlConfigurationSource : FileConfigurationSource
{
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new TomlConfigurationProvider(this);
    }
}