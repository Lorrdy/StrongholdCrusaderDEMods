using BepInEx;
using BepInEx.Logging;
using __GUID__.UI;
using R3;
using SHCDESE.API;
using SHCDESE.API.LowLevel;
using SHCDESE.EventAPI;
using System;

namespace __GUID__
{
    [BepInDependency("000shcdese", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("__GUID__", "__GUID__", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static LobbySettingsViewModel LobbySettingsViewModel { get; private set; }

        private void Awake()
        {
            Logger.LogInfo("Plugin is initializing...");

            LobbySettingsViewModel = new LobbySettingsViewModel();

            GameXAMLManagerAPI.Instance.RegisterLobbyModSettings(
                plugin: this,
                modName: "__GUID__",
                viewModel: LobbySettingsViewModel,
                xamlSourceFile:"XAMLResources/__GUID__Settings.xaml"
            );
            
            // Wait for the C++ library to be ready
            CrusaderLibrary.Instance.LibraryLoaded += OnLibraryLoaded;
        }

        private void OnLibraryLoaded(IntPtr moduleHandle, ReadOnlySpan<byte> memory)
        {
            Logger.LogInfo("Game Library Loaded! APIs are now safe to use.");

            // Register events and initialize logic here
            InitializeGameLogic();
        }

        private void InitializeGameLogic()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error initializing game logic: {ex.Message}");
            }
        }
    }
}
