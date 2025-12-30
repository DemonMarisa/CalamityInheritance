using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class DazzlingStabberBuffLegacy : ModBuff
    {
        public override bool RightClick(int buffIndex)
        {
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.type == ProjectileType<DazzlingStabberProj>() && proj.owner == Main.myPlayer)
                {
                    proj.Kill();
                    Main.player[proj.owner].ClearBuff(buffIndex);
                }
            }
            return true;
        }
    }
}
