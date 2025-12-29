using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class SonYharonBuff: ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (player.ownedProjectileCounts[ProjectileType<SonYharon>()] > 0)
            {
                usPlayer.OwnSonYharon = true;
            }
            if (!usPlayer.OwnSonYharon)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else player.buffTime[buffIndex] = 18000;
        }
    }
}