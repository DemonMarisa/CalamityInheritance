using CalamityInheritance.Content.Projectiles.Summon.Worms;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Summon
{
    public class DOGSummonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //Main.persistentBuff[Type] = true;
        }
        public override bool RightClick(int buffIndex)
        {
            foreach(Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.type == ProjectileType<DOGworm>() && proj.owner == Main.myPlayer)
                {
                    proj.Kill();
                    Main.player[proj.owner].ClearBuff(buffIndex);
                }
            }
            return true;
        }
    }
}
