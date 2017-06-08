cd..
msbuild /t:pack /v:m  
for /r %%b in (pkg\*.nupkg) do copy %%b ..\pkg\