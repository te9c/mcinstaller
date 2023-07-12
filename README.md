# MCInstaller 🛠️

A CLI tool (currently only for Linux, but later maybe with Windows support) for easy installation of minecraft servers.

Using [ServerJars .NET API.](https://github.com/tekgator/ServerJars)

## Currently avaliable server types
- Vanilla
- Forge
- Paper

## Installation
**1. Install** `.NET Core`;  
**2. Clone repository:**
```bash
$ git clone https://github.com/te9c/mcinstaller.git
```
**3. Build:**
```bash
$ cd mcinstaller
$ dotnet build
```
**4. Run with**
```bash
$ dotnet run --project MCInstaller.Console -- <arguments>
```
**Or publish binary and run it**
```bash
$ dotnet publish -o <output folder>
$ cd <output folder>
$ ./mcinstaller <arguments>
```
## Usage
**General use:**

```bash
$ mcinstaller --minecraft-version <version> --type <Vanilla, Forge, Paper> <output folder>
```

MCInstaller requires at least two arguments to be set: `--minecraft-version` (`-m`) and output path (should be empty directory).
If `--type` option not set it defaults to vanilla.
The following command installs vanilla 1.12.2 minecraft server in current folder:
```bash
$ mcinstaller --minecraft-version 1.12.2 .
```

## Options
| Key                   | Short  | Input                                                             | Description                                                                                                                             |
| --------------------- | ------ | ----------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| `None`                | `None` | Empty output folder.                                              | **Required.** Sets output folder                                                                                                        |
| `--minecraft-version` | `-m`   | Minecraft version in format `major.minor.patch` or `major.minor`. | **Required.** Sets minecraft version to install. Available versions are >=1.7.4.                                                        |
| `--type`              | `-t`   | `Vanilla`, `Forge` or `Paper`.                                    | Sets server type to install.                                                                                                            |
| `--java`              | `None` | Path to java.                                                     | (Currently not implemented) Manually specified path to java. By default, MCInstaller tries to find automatically required java version. |
| `--forced`            | `-f`   | None.                                                             | Overrides restriction on non-empty directories. (By default you can install only in empty directories)                                  |
| `--verbose`           | `-v`   | None.                                                             | Sets verbose message output. Mutually exclusive with `--quiet` option.                                                                  |
| `--quiet`             | `-q`   | None.                                                             | Do not print information messages (errors and warns still prints though). Mutually exclusive with `--verbose` option.                   |
| `--help`              | `None` | None.                                                             | Shows help screen.                                                                                                                      |
| `--version`           | `None` | None.                                                             | Prints version.                                                                                                                         |
