using BepInEx;
using BepInEx.Logging;
using LorrdySubject.UI;
using R3;
using SHCDESE.API;
using SHCDESE.API.LowLevel;
using SHCDESE.EventAPI;
using System;

namespace LorrdySubject
{
    [BepInDependency("000shcdese", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("LorrdySubject", "Lorrdy Subject", "1.0.4")]
    public class Plugin : BaseUnityPlugin
    {
        public static LobbySettingsViewModel LobbySettingsViewModel { get; private set; }

        private void Awake()
        {
            Logger.LogInfo("Plugin is initializing...");

            LobbySettingsViewModel = new LobbySettingsViewModel();

            GameXAMLManagerAPI.Instance.RegisterLobbyModSettings(
                plugin: this,
                modName: "Lorrdy Subject",
                viewModel: LobbySettingsViewModel,
                xamlSourceFile:"XAMLResources/LorrdySubjectSettings.xaml"
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
                UnitR3EventHooks.OnUnitTakeMeleeDamage.Observable.Subscribe(UnitEvents.OnUnitTakeMeleeDamage);
                UnitR3EventHooks.OnUnitTakeProjectileDamageEx.Observable.Subscribe(UnitEvents.OnUnitTakeProjectileDamageEx);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error initializing game logic: {ex.Message}");
            }
        }
    }
}
