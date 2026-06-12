using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon.SarosPossessionL;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class SarosPossessionBuffLegacy : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (player.ownedProjectileCounts[ProjectileType<SarosAuraLegacy>()] > 0)
                modPlayer.sarosPossessionLegacy = true;
            if (!modPlayer.sarosPossessionLegacy)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
    }
}
