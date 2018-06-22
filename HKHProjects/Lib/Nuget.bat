NuGet.exe Pack ..\HKH.Common\HKH.Common.csproj -Prop Configuration=Release
NuGet.exe Pack ..\DataProvider\HKH.DataProvider\HKH.DataProvider.csproj -Prop Configuration=Release
NuGet.exe Pack ..\DataProvider\HKH.DataProvider.Dapper\HKH.DataProvider.Dapper.csproj -Prop Configuration=Release
::NuGet.exe Pack ..\DataProvider\HKH.DataProvider.SqlDatabase\HKH.DataProvider.SqlDatabase.csproj -Prop Configuration=Release
NuGet.exe Pack ..\HKH.AOP\HKH.AOP.csproj -Prop Configuration=Release
NuGet.exe Pack ..\HKH.CSV\HKH.CSV.csproj -Prop Configuration=Release
NuGet.exe Pack ..\HKH.Exchange\HKH.Exchange.csproj -Prop Configuration=Release
NuGet.exe Pack ..\HKH.Tasks\HKH.Tasks.csproj -Prop Configuration=Release
NuGet.exe Pack ..\HKH.WCF\HKH.WCF.csproj -Prop Configuration=Release
NuGet.exe Pack ..\Unity.Web\Unity.Web.csproj -Prop Configuration=Release
NuGet.exe Pack ..\MEF.Web\MEF.Web.csproj -Prop Configuration=Release
NuGet.exe Pack ..\DataProvider\DapperLinq\HKH.Linq\HKH.Linq.csproj -Prop Configuration=Release
NuGet.exe Pack ..\DataProvider\DapperLinq\HKH.Linq.Dapper\HKH.Linq.Dapper.csproj -Prop Configuration=Release
NuGet.exe Pack ..\DataProvider\DapperLinq\HKH.Linq.SqlServer\HKH.Linq.SqlServer.csproj -Prop Configuration=Release

Del ..\ReleasePackages\*.*
Move *.nupkg ..\ReleasePackages\

::NuGetPackageUploader.exe ..\ReleasePackages\ /a

pause