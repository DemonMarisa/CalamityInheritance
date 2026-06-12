using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Wulfrum;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class WulfrumDroidOldBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (player.ownedProjectileCounts[ProjectileType<WulfrumDroidOld>()] > 0)
            {
                modPlayer.wulfrumDroidOld = true;
            }
            if (!modPlayer.wulfrumDroidOld)
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
