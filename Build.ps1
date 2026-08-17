$ProjectFile = "SplasherArchipelago.slnx"
$Configuration = "Debug"
$PluginDir = "C:\Program Files (x86)\Steam\steamapps\common\Splasher\BepInEx\plugins"

Write-Host "Starting build for $ProjectFile ($Configuration)..." -ForegroundColor Cyan

# 1. Execute the build command
msbuild $ProjectFile /p:Configuration=$Configuration /verbosity:minimal

# 2. Check if the build succeeded
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed with exit code $LASTEXITCODE. Aborting." -ForegroundColor Red
    exit $LASTEXITCODE
}

Copy-Item -Path "$PSScriptRoot\Archipelago\bin\$Configuration\Archipelago.dll" -Destination $PluginDir -Force
Copy-Item -Path "$PSScriptRoot\Core\bin\$Configuration\Core.dll" -Destination $PluginDir -Force

Write-Host "All tasks completed." -ForegroundColor Cyan