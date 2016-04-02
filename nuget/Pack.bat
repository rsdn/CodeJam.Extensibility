cd ..\Main\src
call Compile.cmd

cd ..\..\NuGet

del *.nupkg

NuGet Pack CodeJam.Main.nuspec
