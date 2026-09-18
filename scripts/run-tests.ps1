$root = Split-Path -Parent $PSScriptRoot

Write-Host "==================================="
Write-Host "WEREWOLF AUTOMATED TEST RUNNER"
Write-Host "==================================="
Write-Host ""

$failed = $false
$serverProcess = $null

# ------------------------------------------------------------
# 1. GAME PACKET TESTS
# ------------------------------------------------------------

Write-Host "[1/3] Running GamePacketTests..."

dotnet test "$root\test\GamePacketTests\GamePacketTests.csproj"

if ($LASTEXITCODE -ne 0) {
	Write-Host "GamePacketTests FAILED."
	$failed = $true
} 
else {
	Write-Host "GamePacketTests PASSED."
}

Write-Host ""

# ------------------------------------------------------------
# 2. GAME LOGIC TESTS
# ------------------------------------------------------------

Write-Host "[2/3] Running GameLogicTests..."

dotnet test "$root\test\GameLogicTests\GameLogicTests.csproj"

if ($LASTEXITCODE -ne 0) {
	Write-Host "GameLogicTests FAILED."
	$failed = $true
} 
else {
	Write-Host "GameLogicTests PASSED."
}

#------------------------------------------------------------
# 3. STRESS TEST - 7 CLIENTS
#------------------------------------------------------------

# =========================================================
# 3. STRESS TEST - 7 CLIENTS
# =========================================================

Write-Host "[3/3] Running Stress Test - 7 Clients..."

$serverProject = Join-Path $root "src\server\WerewolfServer.csproj"
$stressProject = Join-Path $root "test\StressTestClient\StressTestClient.csproj"

$serverProcess = $null

try {
    Write-Host "Starting WerewolfServer..."

    # Tim dotnet.exe tren may hien tai
    $dotnetExe = (Get-Command dotnet).Source

    # Tao process server
    $startInfo = New-Object System.Diagnostics.ProcessStartInfo
    $startInfo.FileName = $dotnetExe
    $startInfo.Arguments = "run --project `"$serverProject`""
    $startInfo.WorkingDirectory = $root

    # Chay ngam, khong mo them cua so
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true

    # Cho phep doc log neu server bi loi
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true

    $serverProcess = New-Object System.Diagnostics.Process
    $serverProcess.StartInfo = $startInfo

    $null = $serverProcess.Start()

    Write-Host "Waiting for server on port 8080..."

    $serverReady = $false

    # Cho toi da 20 giay
    for ($i = 0; $i -lt 20; $i++) {

        Start-Sleep -Seconds 1

        # Neu server tu tat thi bao ngay
        if ($serverProcess.HasExited) {
            Write-Host "WerewolfServer exited unexpectedly."

            $serverOutput = $serverProcess.StandardOutput.ReadToEnd()
            $serverError = $serverProcess.StandardError.ReadToEnd()

            if ($serverOutput) {
                Write-Host $serverOutput
            }

            if ($serverError) {
                Write-Host $serverError
            }

            break
        }

        try {
            $tcp = New-Object System.Net.Sockets.TcpClient
            $tcp.Connect("127.0.0.1", 8080)
            $tcp.Close()

            $serverReady = $true
            break
        }
        catch {
            Write-Host "Server not ready yet..."
        }
    }

    if (-not $serverReady) {
        Write-Host "WerewolfServer: FAILED TO START."
        $failed = $true
    }
    else {
        Write-Host "WerewolfServer: READY"
        Write-Host "Running StressTestClient..."
        Write-Host ""

        dotnet run `
            --project "$stressProject" `
            -- --ci

        if ($LASTEXITCODE -ne 0) {
            Write-Host ""
            Write-Host "StressTestClient: FAILED"
            $failed = $true
        }
        else {
            Write-Host ""
            Write-Host "StressTestClient: PASSED"
        }
    }
}
catch {
    Write-Host "Stress Test Error:"
    Write-Host $_.Exception.Message

    $failed = $true
}
finally {
    if ($serverProcess -ne $null) {
        if (-not $serverProcess.HasExited) {
            Write-Host "Stopping WerewolfServer..."

            $serverPid = $serverProcess.Id

            taskkill /PID $serverPid /T /F | Out-Null
        }
    }
}

#------------------------------------------------------------
#FINAL RESULT
#------------------------------------------------------------

Write-Host ""
Write-Host "==================================="

if ($failed) {
	Write-Host "AUTOMATED TEST RESULT: FAILED."
	exit 1
} 
else {
	Write-Host "AUTOMATED TEST RESULT: PASSED."
	exit 0
}