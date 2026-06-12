using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class CosmicEnergyExtra : ModBuff
    {
        public override string Texture => "CalamityInheritance/Buffs/Summon/CosmicEnergyOld";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.CIMod();
            if (player.ownedProjectileCounts[ProjectileType<SaveCosmic>()] > 0)
            {
                modPlayer.CosmicEnergyExtra = true;
            }
            if (!modPlayer.CosmicEnergyExtra)
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
