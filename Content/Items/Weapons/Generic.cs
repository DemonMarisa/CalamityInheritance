using CalamityInheritance.NPCs.Boss.SCAL;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons
{
    public class Generic
    {
        public static string WeaponPath => "CalamityInheritance/Content/Items/Weapons";
        public static string WeaponLocal => "Content.Items.Weapons";
        public static string GetWeaponLocal => "Mods.CalamityInheritance.Content.Items.Weapons";
        public static float GenericLegendBuff() => NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()) ? 2.0f : 0f;
        public static int GenericLegendBuffInt(int? buffDamage = 100) => (int)(NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()) ? buffDamage : 0);
        public static int GenericLegendBuffFloat(int? buffDamage = 5) => (int)(NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()) ? buffDamage : 0);
    }
}