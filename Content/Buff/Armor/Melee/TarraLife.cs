using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Armor.Melee
{
    public class TarraLife : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.miscCounter % 15 == 0)
                player.NCHeal(1);
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(player.position - new Vector2(2f), player.width + 4, player.height + 4, DustID.Terra, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.75f;
                Main.dust[dust].velocity.Y -= 0.35f;
                Main.dust[dust].scale = 2f;
            }
        }
    }
}
