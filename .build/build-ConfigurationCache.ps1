<#
    .SYNOPSIS
    Defines build script for the BagOUtils.ConfigurationCache library.

    .DESCRIPTION
    This defines how to build ConfigurationCache and then package the
    assembly into a NuGet package. It also determines if this is an updated
    package and if so, updates NuGet.

    This is a psake script, so it will only run via Invoke-psake (or equivelant).

    This requires certain command line tools be installed. Run the Install-BuildTools.ps1
    in the tools folder to install them.

    .NOTES
        Change Log
        ----------------------------------------------------------------
        2016-10-02 (gpm): Initial create.
        2016-10-04 (gpm): Flushed out actual build
#>


#This build assumes the following directory structure
#
#  \.build          - This is where the project build code lives
#  \[root]          - This is the root folder containing the solution file and the build folders.
#  \build-artifacts - This folder is created if it is missing and contains output of the build
#
Properties {
    $buildFolder = Split-Path $psake.build_script_file
    $rootFolder = [System.IO.Path]::GetFullPath("$buildFolder\..")
    $buildArtifactsFolder = [System.IO.Path]::GetFullPath("$rootFolder\build-artifacts\")
    $solution = "BagOUtils.sln"
    $solutionPath = "$rootFolder\src\$solution"
    $nugetSpec = "ConfigurationCache.nuspec"
    $nugetSpecPath = "$buildArtifactsFolder\$nugetSpec"
    $assemblyName = "BagOUtils.ConfigurationCache"
    $publishAction = $false
    $prereleaseFolder = ""
}

FormatTaskName (("-"*10) + "[ {0} ]" + ("-"*40))

task default -depends Publish

task Clean {
    Write-Host "Creating build-artifacts directory" -ForegroundColor Green
    if (Test-Path $buildArtifactsFolder)
    {
        rd $buildArtifactsFolder -rec -force | out-null
    }

    mkdir $buildArtifactsFolder | out-null

    Write-Host "Cleaning $solution" -ForegroundColor Green
    Exec { msbuild "$solutionPath" /t:Clean /p:Configuration=Release /v:quiet }
}


# Restore NuGet packages
task Restore -depends Clean {
    nuget restore "$solutionPath"
}

task Build -depends Restore {
    Write-Host "Building $solution" -ForegroundColor Green
    Exec { msbuild "$solutionPath" /t:Build /p:Configuration=Release /v:quiet /p:OutDir=$buildArtifactsFolder }
}

# Package into a NuGet Package
task Package -depends Build {
    exec { nuget pack $nugetSpecPath -OutputDirectory $buildArtifactsFolder}

    Get-ChildItem "$buildArtifactsFolder" -Exclude ("*.nupkg") | foreach ($_) {remove-item $_.fullname}
}

# Publish package to NuGet
task Publish -depends Package {
    switch($publishAction)
    {
        "none" { Write-Host "Publish skipped by request." }
        "local" {
            Write-Host "Publish to local pre-release folder."
            Get-ChildItem -Path "$buildArtifactsFolder" -Filter "*.nupkg" `
                | Copy-Item -Destination "$prereleaseFolder" -Force
        }
        "nuget" { Write-Host "Publish to NuGet.com."}
        default { Write-Host "Unknown publish action: $publishAction." }
    }
}