using System;

namespace Server.Mobiles
{
    public static class FamilyPetBonding
    {
        public static void Initialize()
        {
            EventSink.WorldLoad += OnWorldLoad;
        }

        public static bool HasPerfectCare(BaseCreature pet)
        {
            return Config.Get("Family.PerfectPetCare", false) && pet.Controlled &&
                pet.ControlMaster != null && pet.ControlMaster.Player && !pet.Summoned;
        }

        public static void TryBond(BaseCreature pet)
        {
            if (HasPerfectCare(pet))
            {
                pet.Loyalty = BaseCreature.MaxLoyalty;
                pet.Hunger = 20;
                pet.Thirst = 20;
                pet.OwnerAbandonTime = DateTime.MinValue;
            }

            if (Config.Get("Family.InstantPetBonding", false) &&
                pet.Controlled && pet.ControlMaster != null && pet.ControlMaster.Player &&
                pet.IsBondable && !pet.IsBonded)
            {
                pet.IsBonded = true;
                pet.BondingBegin = DateTime.MinValue;
            }
        }

        private static void OnWorldLoad()
        {
            foreach (Mobile mobile in World.Mobiles.Values)
            {
                BaseCreature pet = mobile as BaseCreature;
                if (pet != null)
                {
                    TryBond(pet);
                }
            }
        }
    }
}
