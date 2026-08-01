@echo off

set DEST=workshop\package

if not exist "%DEST%" mkdir "%DEST%"

xcopy "BepInEx" "%DEST%\BepInEx" /E /I /Y

copy "info.json" "%DEST%" /Y
copy "steam-preview.png" "%DEST%" /Y

echo Copy complete.
echo Package now

..\tools\SDK-Workshop-Packager\SHCDESE.WorkshopPackager.exe ^
    -s "workshop\package" ^
    -o "workshop\Releases\mod.map"

echo Package complete.

pause
