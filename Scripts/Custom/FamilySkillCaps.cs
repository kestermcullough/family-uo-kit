using Server.Mobiles;

namespace Server
{
    public static class FamilySkillCaps
    {
        public static void Initialize()
        {
            EventSink.WorldLoad += OnWorldLoad;
            EventSink.Login += OnLogin;
        }

        private static void Apply(Mobile mobile)
        {
            if (!Config.Get("Family.AllSkills120", false) || !(mobile is PlayerMobile))
                return;

            mobile.SkillsCap = Config.Get("PlayerCaps.TotalSkillCap", 1000000);
            for (int i = 0; i < mobile.Skills.Length; i++)
                mobile.Skills[i].Cap = 120.0;
        }

        private static void OnLogin(LoginEventArgs args)
        {
            Apply(args.Mobile);
        }

        private static void OnWorldLoad()
        {
            foreach (Mobile mobile in World.Mobiles.Values)
                Apply(mobile);
        }
    }
}
