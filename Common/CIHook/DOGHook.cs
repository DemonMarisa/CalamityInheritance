using CalamityMod.NPCs.TownNPCs;
using CalamityMod.NPCs;
using CalamityMod;
using Terraria.ModLoader;
using System.Reflection;
using CalamityMod.NPCs.DevourerofGods;
using Microsoft.Xna.Framework;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Common.CIHook
{
    public class DOGHook
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod = typeof(DevourerofGodsHead).GetMethod(nameof(DevourerofGodsHead.OnKill));
            MonoModHooks.Add(originalMethod, OnKill_Hook);
        }

        public static void OnKill_Hook(DevourerofGodsHead self)
        {
            CalamityGlobalNPC.SetNewBossJustDowned(self.NPC);

            CalamityGlobalNPC.SetNewShopVariable(new int[] { ModContent.NPCType<THIEF>() }, DownedBossSystem.downedDoG);

            // If DoG has not been killed yet, notify players that the holiday moons are buffed
            if (!DownedBossSystem.downedDoG)
            {
                string key = "Mods.CalamityMod.Status.Progression.DoGBossText";
                Color messageColor = Color.Cyan;
                string key2 = "Mods.CalamityMod.Status.Progression.DoGBossText2";
                Color messageColor2 = Color.Orange;
                string key3 = "Mods.CalamityMod.Status.Progression.DargonBossText";
                Color messageColor3 = Color.Yellow;

                CalamityUtils.DisplayLocalizedText(key, messageColor);
                CalamityUtils.DisplayLocalizedText(key2, messageColor2);

                if (!CIServerConfig.Instance.SolarEclipseChange)
                    CalamityUtils.DisplayLocalizedText(key3, messageColor3);
            }

            // Mark DoG as dead
            DownedBossSystem.downedDoG = true;
            CalamityNetcode.SyncWorld();
        }
    }
}
