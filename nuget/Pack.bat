cd ..\src
call Compile.cmd

cd ..\NuGet

del *.nupkg

NuGet Pack CodeJam.Extensibility.nuspec
