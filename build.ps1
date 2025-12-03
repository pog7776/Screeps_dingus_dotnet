param ([string]$config = "Debug")

function Copy-Output {
    param ($Destination)

    Write-Output "Copying: $path -> $Destination"
    Copy-Item -Path $path -Destination $Destination -Recurse
}

Write-Output "Building: $config"

dotnet publish -c $config

$path = ".\bin\$config\net8.0\wasi-wasm\AppBundle\world\*"
$output = "C:\Users\jackc\AppData\Local\Screeps\scripts\screeps.com\$config\"
$localDestination = "C:\Users\jackc\AppData\Local\Screeps\scripts\127_0_0_1___21025\$config\"

#Copy-Output $output
Copy-Output $localDestination