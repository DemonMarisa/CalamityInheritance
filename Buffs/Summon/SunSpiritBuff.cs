using CalamityInheritance.Content.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class SunSpiritBuff : ModBuff
    {
        public override bool RightClick(int buffIndex)
        {
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.type == ProjectileType<SunSpiritMinionLegacy>() && proj.owner == Main.myPlayer)
                {
                    proj.Kill();
                    Main.player[proj.owner].ClearBuff(buffIndex);
                }
            }
            return true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 2;
        }
    }
}
