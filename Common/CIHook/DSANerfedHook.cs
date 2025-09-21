using System.Collections.Generic;
using System.Reflection;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CIHook
{
    public class DSANerfedHook
    {
        public static void Load()
        {
            MethodInfo fuckUpdateAcc = typeof(DimensionalSoulArtifact).GetMethod(nameof(DimensionalSoulArtifact.UpdateAccessory));
            MonoModHooks.Add(fuckUpdateAcc, FuckUpdate_AccHook);
        }
        public static void FuckUpdate_AccHook(DimensionalSoulArtifact self, Player player, bool hideVisual)
        {
            Mod mod = CalamityInheritance.WrathoftheGods;
            if (mod != null && CIServerConfig.Instance.CalStatInflationBACK)
            {
                
            }
            else
            {
                player.Calamity().dArtifact = true;
            }
        }
    }
    public class AddDSATooltip : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<DimensionalSoulArtifact>();
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod = CalamityInheritance.WrathoftheGods;
            if (mod is null)
                return;
            bool xerocAndNoxus = mod.AnyWrathBoss("NamelessDeityBoss") || mod.AnyWrathBoss("AvatarOfEmptiness") || mod.AnyWrathBoss("AvatarRift");
            string t = null;
            if (xerocAndNoxus)
                t = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Accessories.DSANerfed");
            if (t is not null)
                tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public static class DSAMethod
    {
        public static bool AnyWrathBoss(this Mod mod, string name)
        {
            if (mod is null)
                return false;
            int wotgBoss = mod.Find<ModNPC>(name).Type;
            if (NPC.AnyNPCs(wotgBoss))
                return true;
            else
                return false;
        }
    }
}