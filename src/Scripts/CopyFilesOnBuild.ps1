#Parameters that we need
param(
    [Parameter(Mandatory=$True)]
    [string] $gameDirectory,

    [Parameter(Mandatory=$True)]
    [string] $projectDirectory
)

class CopyItem
{
    [string] $ItemToCopy
    [string] $WhereToCopyInBuild
}

#Read the json file
$itemsToCopy = [CopyItem[]](Get-Content ($projectDirectory + "/ItemsToCopy.json") | Out-String | ConvertFrom-Json)

#Now go through each item that needs coping, and copy it
foreach ($itemToCopy in $itemsToCopy)
{
    #Make sure the path exists, and if not, create it
    if (!(Test-Path -path ($gameDirectory + $itemToCopy.WhereToCopyInBuild))) {[void](New-Item ($gameDirectory + $itemToCopy.WhereToCopyInBuild) -Type Directory)}

    #Actually copy the item
    Copy-Item ($projectDirectory + $itemToCopy.ItemToCopy) -Destination ($gameDirectory + $itemToCopy.WhereToCopyInBuild) -Recurse -Force

    Write-Output ("Copied: '" + $itemToCopy.ItemToCopy + "' -> '" + $itemToCopy.WhereToCopyInBuild + "'")
}