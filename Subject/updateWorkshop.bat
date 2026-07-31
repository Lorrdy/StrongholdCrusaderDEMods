@echo off

if not exist "workshop\Releases\LorrdySubject.map" (
    echo mod not found!
    echo Maybe you need to repack it first.
    goto not_found
  )

echo Update mod

..\tools\pdengine-steamugc-tool\pdengine.steamugc.tool.exe ^
  -v ^
  -a 3024040 ^
  -i 3671450357 ^
  -u ^
  -s "C:\Documents\Code\StrongholdCrusaderDEMods\Subject\workshop\Releases"

echo Mod updated.
pause
exit /b %BUILD_EXIT_CODE%

:not_found
pause
exit /b 1
