using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace TomlConfigurationPOC;

public static class TomlConfigurationExtensions
{
    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path)
    {
        return AddTomlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
    }

    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, bool optional)
    {
        return AddTomlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
    }

    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
    {
        return AddTomlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
    }

    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional, bool reloadOnChange)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Invalid File Path", nameof(path));
        }

        return builder.AddTomlFile(s =>
        {
            s.FileProvider = provider;
            s.Path = path;
            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
        });
    }

    public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, Action<TomlConfigurationSource> configureSource)
        => builder.Add(configureSource);

    public static IConfigurationBuilder AddTomlStream(this IConfigurationBuilder builder, Stream stream)
    {
        return builder.Add<TomlStreamConfigurationSource>(s => s.Stream = stream);
    }
}