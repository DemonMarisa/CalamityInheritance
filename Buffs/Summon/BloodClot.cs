using CalamityInheritance.Content.Projectiles.Summon;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Summon
{
    public class BloodClot : ModBuff
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
            if (player.ownedProjectileCounts[ProjectileType<BloodClotMinion>()] > 0)
            {
                modPlayer.bloodClot = true;
            }
            if (!modPlayer.bloodClot)
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
