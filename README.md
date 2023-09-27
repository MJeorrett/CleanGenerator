# Clean Generator
Scaffolding tool for clean architecture project in dotnet.

**WORK IN PROGRESS!**

Developed and tested on Windows. May or may not work on Mac & Linux yet.

## Running
### Debug
- Delete the contents of the `./test-output` directory.
- Ensure that any previously created database is deleted if required to avoid out of sync migrations.
- Set debug parameters in launchsettings.json.
- Hit F5.

## Build and install
To build and install the tool globally on your local machine run this from ./CleanGenerator:

> rm .\nupkg\CleanGen.1.0.1.nupkg; dotnet tool uninstall --global CleanGen; dotnet pack; dotnet tool install --global --add-source .\nupkg\ CleanGen

Then you can use it anywhere by running `cleangen ...`.

## TODO

- [ ] Support `DateTime` and `bool` property types.
- [ ] FK relationships.
- [ ] Allow specifying what properties appear on detail and summary dtos.
- [ ] Allow skipping adding migration when adding entity.
- [ ] Allow specifying properties on init entity type.
- [ ] Add `DateTime` kind management in DbContext.
- [ ] Eliminate warnings when running dotnet pack.
- [ ] Install dotnet ef tool locally.
- [ ] Ensure that tool works cross platform, particularly path separators might need attention.
- [x] Create output folder if it doesn't exist.
- [x] Scaffold from entity instead of config.
- [x] Scaffold e2e tests.
- [x] Scaffold validation.
- [x] Add delete command.
- [x] Allow passing in entity properties and types.
	- [x] Allow specifying entity name.
	- [x] Allow specifying properties and types.
	- [x] Allow specifying SQL types.
	- [x] Name PKs in tables correctly e.g. `TodoId` instead of just `Id`.
- [x] Dynamically resolve project name e.g. from <ProjectName>.WebApi.
- [x] Allow specifying the initial entity name when running init.
- [x] Dynamically resolve input directory (to tool root).
- [x] Refactor templates into their own folder.
- [x] Separate commands for scaffold project and CRU endpoint.
- [x] Wrap in tool.