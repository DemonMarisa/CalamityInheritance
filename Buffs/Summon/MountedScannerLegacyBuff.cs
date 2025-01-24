using CalamityInheritance.Content.Projectiles.Summon;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.DraedonsArsenal;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class MountedScannerLegacyBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MountedScannerSummonLegacy>()] > 0)
            {
                modPlayer.mountedScanner = true;
            }
            if (!modPlayer.mountedScanner)
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
