using SHCDESE.API;
using SHCDESE.EventAPI;
using SHCDESE.EventAPI.MapLoader;
using SHCDESE.Lua;
using SHCDESE.API.Logging;
using System;
using SHCDESE.API.Components.Timer;

namespace LorrdyAISharesGold
{
    internal static class ShareGold
    {
        // Create once per class (usually as a static field)
        private static readonly Lazy<ModLogHelper> _log = new Lazy<ModLogHelper>(() => ModLoggerFactory.CreateHelper("LorrdyAIShareGold"));
        private static ModLogHelper Log => _log.Value;

        private const string CALLBACK_NAME = "LorrdyAIShareGold_Timer";

        internal static void OnStartMap(MapStartEventArgs args)
        {
            if (args.Phase == EventHookPhase.Pre)
            {
                return;
            }

            Log.Information("Map started, starting timer");

            // Get the engine instance.
            TimerEngine timerEngine = GameTimeManagerAPI.Instance.GetTimerEngine();

            // Schedule the savable timer.
            string timerHandle = timerEngine.AddRepeatedAction(5000, OnTimerCallback, CALLBACK_NAME);
        }

        internal static void OnTimerCallback() 
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            int[] playersID = playerManager.GetAlivePlayerIds();

            //TODO playersID starts with 0, but player 0 is not real
            for (int i = 0; i < playersID.Length; i++)
            {
                playersID[i] = playersID[i] + 1;
            }

            uint[] playersGold = new uint[playersID.Length];

            for (int i = 0; i < playersID.Length; i++)
            {
                playersGold[i] = playerManager.GetPlayerGold(playersID[i]);
            }

            for (int i = 0; i < playersID.Length; i++)
            {
                if (playerManager.IsAIPlayer(playersID[i]) && playersGold[i] > Plugin.LobbySettingsViewModel.MinGoldToShare)
                {
                    for (int j = 0; j < playersID.Length; j++)
                    {
                        if (i == j)
                            continue;

                        if (playerManager.IsPlayerAlliedTo(playersID[i], playersID[j]) &&
                            playerManager.IsAIPlayer(playersID[j]) && playersGold[j] < Plugin.LobbySettingsViewModel.MaxGoldToGet)
                        {
                            HandleGiveGold(playersID[i], playersID[j]);
                            
                            if (Plugin.LobbySettingsViewModel.ShowMessage)
                            {
                                HandleGiveGoldMessage(playersID[i], playersID[j]);
                            }
                        }
                    }
                }
            }
        }

        private static void HandleGiveGold(int from, int to)
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            int amount = Plugin.LobbySettingsViewModel.GoldAmountToShare;
            playerManager.AddPlayerGold(from, -amount);
            playerManager.AddPlayerGold(to, amount);

            Log.Information($"Player {from} sends {amount} to {to}.");
        }

        private static void HandleGiveGoldMessage(int fromId, int toId)
        {
            string fromName = GetPlayerNameById(fromId);
            string toName = GetPlayerNameById(toId);
            int amount = Plugin.LobbySettingsViewModel.GoldAmountToShare;

            LuaNetworkAPI.SendIngameChatLocal(message: $"sends {amount} gold to {toName}.",
                fromName: fromName,
                fromPlayerId: fromId,
                duration: 20);
        }

        private static string GetPlayerNameById(int playerId)
        {
            string name = GameNetworkAPI.GetPlayerById(playerId)?.playerName;
            name ??= GameAIManagerAPI.Instance.GetCustomAILordNameByPlayerId(playerId);
            name = !string.IsNullOrEmpty(name) ? name : $"Player {playerId}";

            return name;
        }
    }
}
