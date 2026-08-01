@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "PROJECT_DIR=%~dp0"
set "MSBUILD=dotnet"
set "GAME_DIR=%STRONGHOLD_GAME_DIR%"
set "GAME_SCRIPT_EXTENDER_DIR=%GAME_DIR%\BepInEx\plugins\000shcdese"

if not exist "%GAME_DIR%\BepInEx\core\BepInEx.dll" (
  echo BepInEx.dll wurde im Spielordner nicht gefunden:
  echo !GAME_DIR!\BepInEx\core\BepInEx.dll
  echo.
  pause
  exit /b 1
)

if not exist "%GAME_SCRIPT_EXTENDER_DIR%\SHCDESE.dll" (
  echo SHCDESE.dll wurde nicht gefunden:
  echo !GAME_SCRIPT_EXTENDER_DIR!\SHCDESE.dll
  echo.
  pause
  exit /b 1
)

echo Verwende Script Extender Referenzen:
echo !GAME_SCRIPT_EXTENDER_DIR!
echo.

pushd "%PROJECT_DIR%"
%MSBUILD% build LorrdySubject.csproj ^
 /p:Configuration=Debug ^
 /p:GameDir="%GAME_DIR%" ^
 /p:ExtenderDir="%GAME_SCRIPT_EXTENDER_DIR%"
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
popd

echo.
if "%BUILD_EXIT_CODE%"=="0" (
  echo Build erfolgreich.
  echo Kopiere Plugin in den Spielordner...
  set "PLUGIN_NAME=LorrdySubject"
  set "LOCAL_PLUGIN_DIR=%PROJECT_DIR%BepInEx\plugins\!PLUGIN_NAME!"
  set "GAME_PLUGIN_DIR=%GAME_DIR%\BepInEx\plugins\!PLUGIN_NAME!"

  if not exist "!LOCAL_PLUGIN_DIR!\" (
    echo Lokaler Plugin-Ordner wurde nicht gefunden:
    echo !LOCAL_PLUGIN_DIR!
    goto copy_failed
  )

  if exist "!GAME_PLUGIN_DIR!\" (
    for /D %%D in ("!GAME_PLUGIN_DIR!\*") do (
      if /I not "%%~nxD"=="LobbyModSettings" (
        rmdir /S /Q "%%~fD"
        if errorlevel 1 goto copy_failed
      )
    )
    for %%F in ("!GAME_PLUGIN_DIR!\*") do (
      if exist "%%~fF" (
        if not exist "%%~fF\" (
          del /F /Q "%%~fF"
          if errorlevel 1 goto copy_failed
        )
      )
    )
  )
  xcopy "!LOCAL_PLUGIN_DIR!" "!GAME_PLUGIN_DIR!\" /E /I /Y
  if errorlevel 1 goto copy_failed
  echo Plugin kopiert.
) else (
  echo Build fehlgeschlagen. Exit Code: %BUILD_EXIT_CODE%
)
echo.
pause
exit /b %BUILD_EXIT_CODE%

:copy_failed
echo.
echo Kopieren fehlgeschlagen. Ist das Spiel noch gestartet?
echo Beende Stronghold Crusader Definitive Edition und starte build.bat erneut.
echo.
pause
exit /b 1