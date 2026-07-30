using SHCDESE.API;
using SHCDESE.EventAPI;
using SHCDESE.EventAPI.MapLoader;
using SHCDESE.Lua;
using SHCDESE.API.Logging;
using System;
using SHCDESE.API.Components.Timer;

namespace LorrdyAIShareGold
{
    internal static class ShareGold
    {
        // Create once per class (usually as a static field)
        private static readonly Lazy<ModLogHelper> _log = new Lazy<ModLogHelper>(() => ModLoggerFactory.CreateHelper("LorrdyAIShareGold"));
        private static ModLogHelper Log => _log.Value;

        private const uint MAX_TO_GET = 2000;
        private const uint MIN_TO_SEND = 20000;
        private const int AMOUNT_TO_GIVE = 2000;

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
            string timerHandle = timerEngine.AddRepeatedAction(2000, OnTimerCallback, CALLBACK_NAME);
        }

        internal static void OnTimerCallback() 
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            int[] playersID = playerManager.GetAlivePlayerIds();
            //TODO starts with 0, but player 0 is not real for some reason
            uint[] playersGold = new uint[playersID.Length];

            Log.Information("Timer callback");
            Log.Information($"Found {playersID.Length} players");

            for (int i = 0; i < playersID.Length; i++)
            {
                playersGold[i] = playerManager.GetPlayerGold(playersID[i]);
                Log.Information($"{i}: {playersGold[i]}");
            }

            for (int i = 0; i < playersID.Length; i++)
            {
                if (playerManager.IsAIPlayer(playersID[i]) && playersGold[i] > MIN_TO_SEND)
                {
                    for (int j = 0; j < playersID.Length; j++)
                    {
                        if (i == j)
                            continue;

                        if (isSameTeam(playersID[i], playersID[j]) &&
                            playerManager.IsAIPlayer(playersID[j]) && playersGold[j] < MAX_TO_GET)
                        {
                            HandleGiveGold(playersID[i], playersID[j]);
                            HandleGiveGoldMessage(playersID[i], playersID[j]);
                        }
                    }
                }
            }
        }

        private static bool isSameTeam(int id1, int id2)
        {
            GamePlayerManagerAPI playerManagerAPI = GamePlayerManagerAPI.Instance;
            return playerManagerAPI.GetPlayerTeam(id1) == playerManagerAPI.GetPlayerTeam(id2);
        }

        private static void HandleGiveGold(int from, int to)
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            playerManager.AddPlayerGold(from, -AMOUNT_TO_GIVE);
            playerManager.AddPlayerGold(to, AMOUNT_TO_GIVE);
        }

        private static void HandleGiveGoldMessage(int fromId, int toId)
        {
            string fromName = GetPlayerNameById(fromId);
            string toName = GetPlayerNameById(toId);

            LuaNetworkAPI.SendIngameChatLocal(message: $"sends {toName} {AMOUNT_TO_GIVE} gold.",
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
