param(
    [string]$Guid
)

$template = "Template"
$destination = $Guid

Copy-Item $template $destination -Recurse

Get-ChildItem $destination -Recurse | ForEach-Object {

    # Rename files
    if ($_.Name -like "*TemplateMod*") {
        $newName = $_.Name.Replace("TemplateMod", $Guid)
        Rename-Item $_.FullName $newName
    }
}

# Replace placeholders inside files
Get-ChildItem $destination -Recurse -File | ForEach-Object {
    (Get-Content $_.FullName) `
        -replace "__GUID__", $Guid |
        Set-Content $_.FullName
}