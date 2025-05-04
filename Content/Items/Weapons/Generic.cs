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
        public static int GenericLegendBuffInt() => NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()) ? 100 : 0;
    }
}