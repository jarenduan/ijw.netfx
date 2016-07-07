for /d %%a in (.\*.*) do dotnet pack %%a
for /r %%b in (*.nupkg) do copy %%b ..\pkg\
