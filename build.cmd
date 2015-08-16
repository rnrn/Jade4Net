"%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" Build.targets
nuget.exe pack .nuget\Jade4Net.nuspec
pause