using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class SMarbleSwordBuff: ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
             var mp = player.CIMod();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ShrineMarbleSword>()] > 0)
            {
                mp.SMarbleSword= true;
            }
            if (!mp.SMarbleSword)
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