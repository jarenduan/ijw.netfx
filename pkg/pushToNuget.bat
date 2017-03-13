@del *.symbols.nupkg

@echo Start pushing nupkgs to nuget server:

@for %%b in (*.nupkg) do nuget push %%b -Source https://www.nuget.org/api/v2/package

@echo Pushing done.