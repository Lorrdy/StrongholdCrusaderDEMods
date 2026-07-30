using SHCDESE.API;
using SHCDESE.EventAPI;
using SHCDESE.EventAPI.Units;
using SHCDESE.Interop;

namespace LorrdySubject
{
    internal static class UnitEvents
    {
        internal static void OnUnitTakeMeleeDamage(UnitTakeDamageByMeleeEventArgs args)
        {
            if (args.Phase == EventHookPhase.Post || args.Damage < 0)
            {
                return;
            }
            HandleUnitTakeDamage(attackedUnit: args.DamagedUnitId, attackingUnit : args.AttackingUnitId);
        }

        internal static void OnUnitTakeProjectileDamageEx(UnitTakeDamageByProjectileExEventArgs args)
        {
            HandleUnitTakeDamage(attackedUnit: args.AttackedUnitId, attackingUnit: args.AttackingUnitId);
        }

        private static void HandleUnitTakeDamage(int attackedUnit, int attackingUnit)
        {
            GameUnitManagerAPI unitManager = GameUnitManagerAPI.Instance;
            eChimps type = unitManager.GetType(attackedUnit);
            if (type == eChimps.CHIMP_TYPE_LORD)
            {
                HandlingSubjugation.HandleLordTakeDamage(attackedUnit, attackingUnit);
            }
        }
    }
}
