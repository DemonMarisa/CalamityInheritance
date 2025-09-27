using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Statbuffs
{
    public class ShizukuMoonlight: ModBuff, ILocalizedModType
    {
        public enum ClassType
        {
            Melee,
            Magic
        }
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            string dPath  = "Mods.CalamityInheritance.Buffs.ShizukuMoonlight.";
            tip = (dPath + "Default").ToLangValue();
            Player player = Main.LocalPlayer;
            ClassType theClass = player.CIMod().moonClass;
            if (theClass is ClassType.Melee)
            {
                string meleePath = dPath + "Melee";
                tip += "\n" +  meleePath.ToLangValue();
            }
            if (theClass is ClassType.Magic)
            {
                string meleePath = dPath + "Magic";
                tip += "\n" +  meleePath.ToLangValue();
            }
            rare = ModContent.RarityType<ShizukuAqua>();
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().ShizukuMoon = true;
            base.Update(npc, ref buffIndex);
        }
        public override void Update(Player player, ref int buffIndex)
        {
            ref bool active = ref player.CIMod().ShizukuMoon;
            player.CIMod().ShizukuMoon = true;
        }
    }
}