@echo off

if not exist "workshop\Releases\mod.map" (
    echo mod not found!
    echo Maybe you need to repack it first.
    goto not_found
  )

echo Update mod

..\tools\pdengine-steamugc-tool\pdengine.steamugc.tool.exe ^
  -v ^
  -a 3024040 ^
  -i 3774768801 ^
  -u ^
  -s "C:\Documents\Code\StrongholdCrusaderDEMods\AISharesGold\workshop\Releases"

echo Mod updated.
pause
exit /b %BUILD_EXIT_CODE%

:not_found
pause
exit /b 1
