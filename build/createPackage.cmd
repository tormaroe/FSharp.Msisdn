msbuild ..\Msisdn\Msisdn.sln /t:Build /p:Configuration="Release"
..\Msisdn\.nuget\NuGet.exe pack FSharp.Msisdn.nuspec