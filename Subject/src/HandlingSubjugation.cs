using SHCDESE.API;
using SHCDESE.EventAPI;
using SHCDESE.EventAPI.Units;
using SHCDESE.Extensions;
using SHCDESE.Interop;
using SHCDESE.Lua;

namespace LorrdySubject
{
    internal static class HandlingSubjugation
    {
        internal static void HandleLordTakeDamage(int attackedUnit, int attackingUnit)
        {
            GameUnitManagerAPI unitManager = GameUnitManagerAPI.Instance;
            int attackedUnitPlayerId = unitManager.GetOwner(attackedUnit);
            int attackingUnitPlayerId = unitManager.GetOwner(attackingUnit);

            int maxHealth = unitManager.GetMaxHealth(attackedUnit);
            int currentHealth = unitManager.GetCurrentHealth(attackedUnit);
            int minHealth = maxHealth / 4; // 25% of default health
            
            if (currentHealth < minHealth)
            {
                unitManager.SetCurrentHealth(attackedUnit, maxHealth);
                HandleTeamSwitch(attackedUnitPlayerId, attackingUnitPlayerId);
                HandleGiveGold(attackedUnitPlayerId, attackingUnitPlayerId);
                HandleGiveGoods(attackedUnitPlayerId);
            }
        }

        private static void HandleGiveGold(int attackedPlayerId, int attackingPlayerId)
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            playerManager.AddPlayerGold(attackedPlayerId, Plugin.LobbySettingsViewModel.GoldForGettingSubjugated);
            playerManager.AddPlayerGold(attackingPlayerId, Plugin.LobbySettingsViewModel.GoldForSubjugating);
        }

        private static void HandleGiveGoods(int attackedPlayerId)
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            playerManager.AddIncomingGood(attackedPlayerId, eGoods.STORED_WOOD_PLANKS, Plugin.LobbySettingsViewModel.WoodForGettingSubjugated);
            playerManager.AddIncomingGood(attackedPlayerId, eGoods.STORED_STONE_BLOCKS, Plugin.LobbySettingsViewModel.StoneForGettingSubjugated);
            playerManager.AddIncomingGood(attackedPlayerId, eGoods.STORED_FOOD_BREAD, Plugin.LobbySettingsViewModel.FoodForGettingSubjugated);
        }

        private static void HandleTeamSwitch(int attackedPlayerId, int attackingPlayerId)
        {
            GamePlayerManagerAPI playerManager = GamePlayerManagerAPI.Instance;
            int team = playerManager.GetPlayerTeam(attackingPlayerId);
            playerManager.SetPlayerTeam(attackedPlayerId, team);


            HandleSubjugationMessage(attackedPlayerId, attackingPlayerId);
        }

        private static void HandleSubjugationMessage(int attackedPlayerId, int attackingPlayerId)
        {
            string attackedName = GetPlayerNameById(attackedPlayerId);
            string attackingName = GetPlayerNameById(attackingPlayerId);

            LuaNetworkAPI.SendIngameChatLocal(message: $"was subjugated by {attackingName}. They now share a team.",
                fromName: attackedName,
                fromPlayerId: attackedPlayerId,
                duration: 40);

            LuaNetworkAPI.SendIngameChatLocal(message: $"gets {Plugin.LobbySettingsViewModel.GoldForSubjugating} gold for winning!",
                fromName: attackingName,
                fromPlayerId: attackingPlayerId,
                duration: 40);

            LuaNetworkAPI.SendIngameChatLocal(message: $"gets some goods to start again!",
                fromName: attackedName,
                fromPlayerId: attackedPlayerId,
                duration: 40);
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
