# Hey Siri

A command line tool help me to do some tasks: glados check in, bandwagon vm status.

# How to use

## Run directly

```
$ dotnet heysiri.dll

Description:

Usage:
  heysiri [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  bandwagon
  glados
```

## Docker

```bash
docker run --rm -v /root/heysiri/appsettings.json:/App/appsettings.json doghappy/heysiri bandwagon status
docker run --rm -v /root/heysiri/appsettings.json:/App/appsettings.json doghappy/heysiri glados checkin
```


# Configuration

You can change appsettings.json to define some default args.