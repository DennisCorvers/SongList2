@echo off
:: Create a backup of patch.ps1 as patch2.ps1
if exist patch.ps1 (
    echo Backing up patch.ps1 as patch2.ps1...
    copy /Y patch.ps1 patch2.ps1
) else (
    echo patch.ps1 does not exist. Skipping backup.
)

:: Run the PowerShell script (patch.ps1)
echo Running update script (patch.ps1)...
powershell -ExecutionPolicy Bypass -File patch.ps1

:: Check if the script executed successfully
if %ERRORLEVEL% NEQ 0 (
    echo The update script failed to run.
    exit /b %ERRORLEVEL%
) else (
    echo Update script completed successfully.
)

:: Delete the backup patch2.ps1 after the update
if exist patch2.ps1 (
    echo Deleting backup patch2.ps1...
    del /F /Q patch2.ps1
) else (
    echo Backup patch2.ps1 not found. Skipping deletion.
)

pause
