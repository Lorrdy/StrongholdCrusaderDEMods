using BepInEx;
using SHCDESE.EventAPI;
using SHCDESE.API.LowLevel;
using System;
using R3;
using LorrdyAISharesGold.UI;
using SHCDESE.API;

namespace LorrdyAISharesGold
{
    [BepInDependency("000shcdese", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("LorrdyAISharesGold", "Lorrdy AI Shares Gold", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static LobbySettingsViewModel LobbySettingsViewModel { get; private set; }

        private void Awake()
        {
            Logger.LogInfo("Plugin is initializing...");

            LobbySettingsViewModel = new LobbySettingsViewModel();

            GameXAMLManagerAPI.Instance.RegisterLobbyModSettings(
                plugin: this,
                modName: "Lorrdy AI Shares Gold",
                viewModel: LobbySettingsViewModel,
                xamlSourceFile:"XAMLResources/LorrdyAISharesGoldSettings.xaml"
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
                MapLoaderR3EventHooks.OnStartMap.Observable.Subscribe(ShareGold.OnStartMap);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error initializing game logic: {ex.Message}");
            }
        }
    }
}