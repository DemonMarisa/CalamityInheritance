using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.TownNPCs;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria.ModLoader;

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
            if (!BossRushEvent.BossRushActive)
            {
                CalamityGlobalNPC.SetNewBossJustDowned(self.NPC);
                CalamityGlobalTownNPC.SetNewShopVariable(new int[1] { ModContent.NPCType<Bandit>() }, DownedBossSystem.downedDoG);
                if (!DownedBossSystem.downedDoG)
                {
                    string key = "Mods.CalamityMod.Status.Progression.DoGBossText";
                    Color cyan = Color.Cyan;
                    string key2 = "Mods.CalamityMod.Status.Progression.DoGBossText2";
                    Color orange = Color.Orange;
                    Color yellow = Color.Yellow;
                    LAPUtilities.DisplayLocalizedText(key, cyan);
                    LAPUtilities.DisplayLocalizedText(key2, orange);
                    if (!CIServerConfig.Instance.SolarEclipseChange)
                        LAPUtilities.DisplayLocalizedText("Mods.CalamityMod.Status.Progression.DargonBossText", yellow);
                }

                DownedBossSystem.downedDoG = true;
                CalamityNetcode.SyncWorld();
            }
        }
    }
}
