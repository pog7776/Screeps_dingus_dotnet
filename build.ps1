param ([string]$config = "Debug")

Write-Output "Building: $config"

dotnet publish -c $config

$path = ".\bin\$config\net8.0\wasi-wasm\AppBundle\world\*"
$destination = "C:\Users\jackc\AppData\Local\Screeps\scripts\127_0_0_1___21025\$config\"

Write-Output "Copying: $path -> $destination"

Copy-Item -Path $path -Destination $destination -Recurse