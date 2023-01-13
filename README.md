# Clean Generator
Scaffolding tools for clean architecture project in dotnet.

**WORK IN PROGRESS!**

Developed and tested on Windows. May or may not work on Mac & Linux yet.

## Running
### Debug
- Delete the contents of the `./test-output` directory.
- Ensure that any previously created database is deleted if required to avoid out of sync migrations.
- Set debug parameters in launchsettings.json.
- Hit F5.

## Build and install
To build and install the tool globally on your local machine
```
> rm .\nupkg\CleanGenerator.0.0.1.nupkg; dotnet tool uninstall --global CleanGenerator; dotnet pack; dotnet tool install --global --add-source .\nupkg\ CleanGenerator
```

Then you can use it anywhere by running `cleangen ...`.

## TODO
- [ ] Allow passing in entity properties and types.
- [ ] Add delete command.
- [ ] Scaffold validation.
- [ ] Scaffold e2e tests.
- [ ] FK relationships.
- [ ] Allow specifying what properties appear on detail and summary dtos.
- [ ] Eliminate warnings when running dotnet pack.
- [ ] Install dotnet ef tool locally.
- [ ] Ensure that tool works cross platform, particularly path separators might need attention.
- [x] Dynamically resolve project name e.g. from <ProjectName>.WebApi.
- [x] Allow specifying the initial entity name when running init.
- [x] Dynamically resolve input directory (to tool root).
- [x] Refactor templates into their own folder.
- [x] Separate commands for scaffold project and CRU endpoint.
- [x] Wrap in tool.