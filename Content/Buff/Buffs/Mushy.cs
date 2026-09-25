using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class Mushy : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 3;
            player.LAP().LifeRegen += 2;
            player.GetDamage<GenericDamageClass>() += 0.1f;
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-20f, 20f)), DustID.BlueFairy, Vector2.Zero, 100, default, 0.9f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.velocity.Y -= 0.1f;
                dust.alpha = 200;
            }
            if (Main.rand.NextBool(15))
            {
                Dust dust2 = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-10f, 10f), 19), Main.rand.NextBool(3) ? 41 : 56, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, -2f)) + player.velocity / 3, 0, default, 0.9f);
                dust2.alpha = 145;
            }
        }
    }
}
