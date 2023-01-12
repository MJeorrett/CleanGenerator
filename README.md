# Clean Generator

**WORK IN PROGRESS!**

Scaffolding tools for clean architecture project in dotnet.

## Running
Build and install the tool globally on your local machine:
```
> rm .\nupkg\CleanGenerator.0.0.1.nupkg; dotnet tool uninstall --global CleanGenerator; dotnet pack; dotnet tool install --global --add-source .\nupkg\ CleanGenerator
```

Then you can use it anywhere by runing `cleangen ...`.

## TODO
- [ ] Dynamically resolve project name e.g. from <ProjectName.WebApi.
- [ ] Allow specifying the initial entity name when running init.
- [ ] Allow specifying what properties appeear on detail and summary dtos.
- [ ] Eliminate warnings when running dotnet pack.
- [ ] Add delete command.
- [ ] Install dotnet ef tool locally.
- [ ] Allow passing in properties and types.
- [ ] Scaffold calidation.
- [ ] Scaffold e2e tests.
- [ ] FK relationships.
- [x] Dynamically resolve input directory (to tool root).
- [x] Refactor templates into their own folder.
- [x] Separate commands for scaffold project and CRU endpoint.
- [x] Wrap in tool.