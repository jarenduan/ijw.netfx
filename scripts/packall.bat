cd /d %~dp0..
msbuild /t:pack /v:m  
for /r %%b in (debug\*.nupkg) do copy %%b %~dp0..\pkg\