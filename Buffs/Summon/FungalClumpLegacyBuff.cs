using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class FungalClumpLegacyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            var usPlayer = player.CIMod();
            bool minion = usPlayer.FungalClumpLegacySummonBuff;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<FungalClumpLegacyMinion>()] > 0)
            {
                minion = true;
            }
            if (!minion)
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