using BepInEx;
using SHCDESE.EventAPI;
using SHCDESE.API.LowLevel;
using System;
using R3;

namespace LorrdyAIShareGold
{
    [BepInDependency("000shcdese", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("LorrdyAIShareGold", "AI Share Gold", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Plugin is initializing...");

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