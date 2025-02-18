# Check if patch-config.txt exists
if (-Not (Test-Path "patch-config.txt")) {
    Write-Host "patch-config.txt does not exist. Exiting."
    exit
}

# Read configuration from patch-config.txt into variables
$patchConfig = Get-Content "patch-config.txt" | ConvertFrom-StringData
$username = $patchConfig.username
$repo = $patchConfig.repo

# Fetch the latest release information from GitHub API
$releaseInfo = Invoke-RestMethod -Uri "https://api.github.com/repos/$username/$repo/releases/latest"
$publishedAt = $releaseInfo.published_at

# Check if previous_patch_version.txt exists
if (Test-Path "previous_patch_version.txt") {
    # Read the previous patch version's datetime (as a string)
    $previousPatchDate = Get-Content "previous_patch_version.txt" -Raw

    # Compare the publishedAt with the previous patch date
    if ([datetime]::Parse($publishedAt) -le [datetime]::Parse($previousPatchDate)) {
        Write-Host "No update needed. Files are up to date. Exiting."
        exit
    }
} else {
    Write-Host "Previous patch version does not exist. Proceeding with update."
}


# Find the `.rar` asset in the release
$rarAsset = $releaseInfo.assets | Where-Object { $_.name -like "*.rar" }

if ($null -eq $rarAsset) {
    Write-Host "No .rar file found in the release assets. Exiting."
    exit
}

# Get the download URL for the .rar file
$rarDownloadUrl = $rarAsset.browser_download_url
Write-Host "Found .rar asset. Downloading from: $rarDownloadUrl"

# Download the .rar file from the release
Invoke-WebRequest -Uri $rarDownloadUrl -OutFile "newversion.rar"

# Function to extract with 7-Zip
function Extract-With7Zip {
    $sevenZipPath = "C:\Program Files\7-Zip\7z.exe"
    
    if (Test-Path $sevenZipPath) {
        Write-Host "Attempting to extract using 7-Zip..."
        & $sevenZipPath x "newversion.rar" -o. -y
        return $true
    } else {
        Write-Host "7-Zip not found. Proceeding to next method."
        return $false
    }
}

# Function to extract with WinRAR
function Extract-WithWinRAR {
    $winRARPath = "C:\Program Files\WinRAR\winrar.exe"
    
    if (Test-Path $winRARPath) {
        Write-Host "Attempting to extract using WinRAR..."

        Start-Process -FilePath $winRARPath -ArgumentList "x", "-o+", "newversion.rar", "." -NoNewWindow -Wait
        return $true
    } else {
        Write-Host "WinRAR not found. Proceeding to next method."
        return $false
    }
}

# Function to extract with Expand-Archive (if .zip file)
function Extract-WithExpandArchive {
    Write-Host "Attempting to extract using Expand-Archive (for .zip files)..."
    try {
        Expand-Archive -Path "newversion.rar" -DestinationPath . -Force
        return $true
    } catch {
        Write-Host "Expand-Archive failed. Could not extract .rar as .zip. Exiting."
        return $false
    }
}

# Try extracting with 7-Zip
if (-not (Extract-With7Zip)) {
    # If 7-Zip fails, try WinRAR
    if (-not (Extract-WithWinRAR)) {
        # If WinRAR fails, try Expand-Archive (for .zip files)
        if (-not (Extract-WithExpandArchive)) {
            Write-Host "All extraction methods failed. Exiting."
            exit
        }
    }
}

# Delete the .rar file after extraction
Remove-Item "newversion.rar" -Force

# Update previous patch version file with the latest patch version
$publishedAt | Out-File "previous_patch_version.txt" -Force

Write-Host "Patch applied successfully."
