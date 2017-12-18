@echo -start pushing nupkgs to nuget server:
for %%b in (*.nupkg) do nuget push %%b -source http://10.230.41.8/ijwserver/api/v2/package
@echo -nupkgs push done!
@echo.
@echo -start push symbols to symbol server:
for %%b in (*.symbols.nupkg) do nuget push %%b -source http://10.230.41.8/ijwsymbol/nuget
@echo -symbols push done!
pause