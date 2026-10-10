using System;

using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public static class FamilyCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("FamilyHelp", AccessLevel.GameMaster, FamilyHelp_OnCommand);
            CommandSystem.Register("GiveSkills", AccessLevel.GameMaster, GiveSkills_OnCommand);
            CommandSystem.Register("GiveStats", AccessLevel.GameMaster, GiveStats_OnCommand);
            CommandSystem.Register("GiveGold", AccessLevel.GameMaster, GiveGold_OnCommand);
            CommandSystem.Register("MakeGM", AccessLevel.Owner, MakeGM_OnCommand);
        }

        private static void FamilyHelp_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage(68, "Family shard commands:");
            e.Mobile.SendMessage("[GiveSkills [value] - set every skill on a targeted player (default 100)");
            e.Mobile.SendMessage("[GiveStats [value] - set STR, DEX, and INT on a targeted player (default 100)");
            e.Mobile.SendMessage("[GiveGold [amount] - place a bank check in a targeted player's bank (default 100000)");
            e.Mobile.SendMessage("[MakeGM - promote a targeted player to Game Master (Owner only)");
        }

        private static void GiveSkills_OnCommand(CommandEventArgs e)
        {
            double value = e.Length > 0 ? e.GetDouble(0) : 100.0;

            if (value < 0.0 || value > 120.0)
            {
                e.Mobile.SendMessage("Skill value must be between 0 and 120.");
                return;
            }

            e.Mobile.SendMessage("Target the player whose skills you want to set to {0}.", value);
            e.Mobile.Target = new FamilyTarget(target =>
            {
                for (int i = 0; i < target.Skills.Length; i++)
                {
                    target.Skills[i].Cap = Math.Max(target.Skills[i].Cap, value);
                    target.Skills[i].Base = value;
                }

                target.SkillsCap = Math.Max(target.SkillsCap, (int)(target.Skills.Length * value * 10.0));
                target.SendMessage(68, "Your skills were set to {0} by {1}.", value, e.Mobile.Name);
                e.Mobile.SendMessage("Set every skill on {0} to {1}.", target.Name, value);
            });
        }

        private static void GiveStats_OnCommand(CommandEventArgs e)
        {
            int value = e.Length > 0 ? e.GetInt32(0) : 100;

            if (value < 1 || value > 250)
            {
                e.Mobile.SendMessage("Stat value must be between 1 and 250.");
                return;
            }

            e.Mobile.SendMessage("Target the player whose STR, DEX, and INT you want to set to {0}.", value);
            e.Mobile.Target = new FamilyTarget(target =>
            {
                target.StatCap = Math.Max(target.StatCap, value * 3);
                target.RawStr = value;
                target.RawDex = value;
                target.RawInt = value;
                target.Hits = target.HitsMax;
                target.Stam = target.StamMax;
                target.Mana = target.ManaMax;
                target.SendMessage(68, "Your stats were set to {0} by {1}.", value, e.Mobile.Name);
                e.Mobile.SendMessage("Set STR, DEX, and INT on {0} to {1}.", target.Name, value);
            });
        }

        private static void GiveGold_OnCommand(CommandEventArgs e)
        {
            int amount = e.Length > 0 ? e.GetInt32(0) : 100000;

            if (amount < 1 || amount > 100000000)
            {
                e.Mobile.SendMessage("Gold amount must be between 1 and 100,000,000.");
                return;
            }

            e.Mobile.SendMessage("Target the player who should receive a {0:N0} gold bank check.", amount);
            e.Mobile.Target = new FamilyTarget(target =>
            {
                target.BankBox.DropItem(new BankCheck(amount));
                target.SendMessage(68, "A {0:N0} gold bank check was placed in your bank by {1}.", amount, e.Mobile.Name);
                e.Mobile.SendMessage("Placed a {0:N0} gold bank check in {1}'s bank.", amount, target.Name);
            });
        }

        private static void MakeGM_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target the player to promote to Game Master.");
            e.Mobile.Target = new FamilyTarget(target =>
            {
                target.AccessLevel = AccessLevel.GameMaster;
                target.SendMessage(68, "You are now a Game Master. Type [FamilyHelp for the family commands.");
                e.Mobile.SendMessage("Promoted {0} to Game Master.", target.Name);
            });
        }

        private sealed class FamilyTarget : Target
        {
            private readonly Action<Mobile> m_Callback;

            public FamilyTarget(Action<Mobile> callback)
                : base(-1, false, TargetFlags.None)
            {
                m_Callback = callback;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Mobile mobile = targeted as Mobile;

                if (mobile == null || !mobile.Player)
                {
                    from.SendMessage("Please target a player character.");
                    return;
                }

                m_Callback(mobile);
            }
        }
    }
}
