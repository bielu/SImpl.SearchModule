$nuspecs =  Get-ChildItem -Path ..\*\*.csproj -Recurse -Force;
$nuspecs | Foreach-Object { 
 dotnet pack $_.FullName  --output "./" --include-source --configuration "Debug"
 nuget.exe push -Source "Beta" -ApiKey az  $_.Name.replace(".csproj",".1.0.0-beta.300.nupkg")
 }
