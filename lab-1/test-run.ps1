$ErrorActionPreference = 'Stop'

$dotnet = 'C:\Program Files\dotnet\dotnet.exe'
$project = 'c:\Users\ivorz\webapp-ASP.NET_MVC_TEH-lab\lab-1\Vjezba.Tests\Vjezba.Tests.csproj'
$stdoutLog = 'c:\Users\ivorz\webapp-ASP.NET_MVC_TEH-lab\lab-1\test-output-final.txt'
$stderrLog = 'c:\Users\ivorz\webapp-ASP.NET_MVC_TEH-lab\lab-1\test-error-final.txt'
$exitCodeFile = 'c:\Users\ivorz\webapp-ASP.NET_MVC_TEH-lab\lab-1\test-exit-final.txt'

Start-Process -FilePath $dotnet -ArgumentList @('test', $project, '--logger', 'console;verbosity=minimal') -RedirectStandardOutput $stdoutLog -RedirectStandardError $stderrLog -Wait | Out-Null
Set-Content -Encoding utf8 $exitCodeFile $LASTEXITCODE