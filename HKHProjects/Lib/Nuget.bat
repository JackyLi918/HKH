dotnet pack ..\HKH.Common\HKH.Common.csproj -c Release
dotnet pack ..\DataProvider\HKH.DataProvider\HKH.DataProvider.csproj -c Release
dotnet pack ..\DataProvider\HKH.DataProvider.Dapper\HKH.DataProvider.Dapper.csproj -c Release
dotnet pack ..\DataProvider\HKH.DataProvider.SqlServer\HKH.DataProvider.SqlServer.csproj -c Release
dotnet pack ..\DataProvider\HKH.DataProvider.MsSqlServer\HKH.DataProvider.MsSqlServer.csproj -c Release
dotnet pack ..\DataProvider\HKH.DataProvider.Odbc\HKH.DataProvider.Odbc.csproj -c Release
dotnet pack ..\HKH.CSV\HKH.CSV.csproj -c Release
::dotnet pack ..\HKH.Exchange\HKH.Exchange.csproj -c Release
dotnet pack ..\HKH.Tasks\HKH.Tasks.csproj -c Release
dotnet pack ..\HKH.CurrencyFormat\HKH.CurrencyFormat.csproj -c Release

dotnet pack ..\HKH.Mef2\HKH.Mef2.Integration.Abstractions\HKH.Mef2.Integration.Abstractions.csproj -c Release
dotnet pack ..\HKH.Mef2\HKH.Mef2.Integration.Autofac\HKH.Mef2.Integration.Autofac.csproj -c Release
dotnet pack ..\Microsoft.PinYinConverter.Core\Microsoft.PinYinConverter.Core.csproj -c Release

Del ..\ReleasePackages\*.*
Move *.nupkg ..\ReleasePackages\

::NuGetPackageUploader.exe ..\ReleasePackages\ /a

pause