$api_name="DiveHub.ClientApi"
$file_name = "DiveHubAPI.json"

docker run --rm -v ${PWD}:/local openapitools/openapi-generator-cli generate `
     -i /local/$file_name `
     -g csharp `
     -o /local/$api_name `
     --additional-properties=packageName=$api_name,netCoreProjectFile=true,modelPropertyNaming=original,targetFramework=net8.0

Start-Sleep -Seconds 1

$ready = $false
while (-not $ready) {
    try {
        Copy-Item -Path $api_name/src/$api_name -Destination ..\..\DiveHubFrontend\. -Force -Recurse
        $ready = $true
    } catch {
        Start-Sleep -Seconds 5
    }
}

Start-Sleep -Seconds 2

$ready = $false
while (-not $ready) {
    try {
        Remove-Item -Path $api_name -Recurse
        $ready = $true
    } catch {
        Start-Sleep -Seconds 1
    }
}