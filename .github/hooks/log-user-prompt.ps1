$logPath = 'C:\Users\ivorz\webapp-ASP.NET_MVC_TEH-lab\lab-1\agent_log.txt'

$payload = $input | Out-String
if (-not [string]::IsNullOrWhiteSpace($payload)) {
    Add-Content -Path $logPath -Value $payload
}
