using System;
using System.Reflection;
using Terraria.ModLoader;

namespace CalamityInheritance.Music.CrossMod
{
    public class CalamityCompatibility : ModSystem
    {
        /// <summary>
        /// Fuck You Calamity
        /// </summary>
        public static Mod CalamityMod { get; set; }

        #region Mechs types
        public static int DoGHead{ get; private set; }


        public static readonly int[] ExoMechNpcTypes =
        [
            DoGHead
            // DoGHeadP2,
            // DoGBody,
            // DoGBodyP2,
            // DoGTail,
            // DoGTailP2,
        ];

        #endregion

        public static bool BossRushActive
        {
            get
            {
                // Don't do anything if clam is not loaded
                if (CalamityMod is null)
                    return false;

                // Obtain type
                Type calamityEventsType = CalamityMod.Code.GetType("CalamityMod.Events.BossRushEvent");
                if (calamityEventsType is not null)
                {
                    // Obtain field and set it here
                    FieldInfo bossRushActiveField = calamityEventsType.GetField("BossRushActive", BindingFlags.Public | BindingFlags.Static);
                    if (bossRushActiveField is not null)
                    {
                        return (bool)bossRushActiveField.GetValue(null);
                    }
                }

                return false;
            }
        }

        public override void Load()
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod clam))
            {
                CalamityMod = clam;

                if (CalamityMod.TryFind("DevourerofGodsHead", out ModNPC devourerofGods)) DoGHead = devourerofGods.Type;
            }
        }

        public override void Unload()
        {
            CalamityMod = null;
        }
    }
}
