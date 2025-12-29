using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Summon
{
    public class CosmicEnergyOld : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (player.ownedProjectileCounts[ProjectileType<CosmicEnergySpiralOld>()] > 0)
            {
                modPlayer.cosmicEnergy = true;
            }
            if (!modPlayer.cosmicEnergy)
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
