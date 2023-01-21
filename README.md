<h1 align="center">
  YuKitsune.Configuration.Env
</h1>

<h3 align="center">
  ENV configuration provider implementation for Microsoft.Extensions.Configuration.

  [![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/YuKitsune/Configuration.Env/ci.yml?branch=main)](https://github.com/YuKitsune/Configuration.Env/actions/workflows/ci.yml)
  [![Nuget](https://img.shields.io/nuget/v/YuKitsune.Configuration.Env)](https://www.nuget.org/packages/yukitsune.configuration.env)
</h3>

## Installation

You can install the package via the .NET Core CLI:
```
dotnet add package YuKitsune.Configuration.Env
```

## Usage

### 1. Create the `.env` file

Create a `.env` file in the root of your project.

Note: Any file with `.env` in the name will get copied to the output directory. 
Example: `.env`, `dev.env`, `.env.prod`, etc.

### 2. Configure the host builder

```cs
public static IHostBuilder CreateHostBuilder(string[] args)
{
  Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
      {
        config.AddEnvFile(".env");
      })
    .ConfigureWebHostDefaults(webBuilder =>
      {
        webBuilder.UseStartup<Startup>();
      });
}
```

That's it! Your program is now reading from `.env` files!

For more information about how configuration works in .NET Core (and in ASP.NET Core specifically), please see the [official docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0).

## ENV file format

The `.env` file parser is designed to be compatable with [docker compose env files](https://docs.docker.com/compose/env-file), and thus uses the same format.

> The following syntax rules apply to the `.env` file:
> - Compose expects each line in an env file to be in `VAR=VAL` format.
> - Lines beginning with `#` are processed as comments and ignored.
> - Blank lines are ignored.
> - There is no special handling of quotation marks. This means that they are part of the `VAL`.

For more information, please see the [official docs](https://docs.docker.com/compose/env-file).

## Caveats

This implementation works similarly to how the implementation for [Environment Variables](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#environment-variables) works.

> The `:` separator doesn't work with environment variable hierarchical keys on all platforms. `__`, the double underscore, is:
> - Supported by all platforms. For example, the `:` separator is not supported by Bash, but `__` is.
> - Automatically replaced by a `:`

For more information, please see the [official docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#environment-variables).

## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

## Credits

This implementation is heavily based on the [`Microsoft.Extensions.Configurations.Ini` package](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Configuration.Ini). Special thanks to all those who've contributed to it!

.NET Core Runtime repo: https://github.com/dotnet/runtime

Original source: https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Configuration.Ini
