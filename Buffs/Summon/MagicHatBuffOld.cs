using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon.Umbrella;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class MagicHatBuffOld : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            if (player.ownedProjectileCounts[ProjectileType<MagicHatOld>()] > 0)
            {
                modPlayer1.MagicHatOld = true;
            }
            if (!modPlayer1.MagicHatOld)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
