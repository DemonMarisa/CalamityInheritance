using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Armor.Melee
{
    public class BloodflareBloodFrenzy : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<MeleeDamageClass>() += 0.25f;
            player.GetCritChance<MeleeDamageClass>() += 25;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
            player.CI().ContactDamageReduction *= 0.5f;

            for (int j = 0; j < 2; j++)
            {
                Dust blood = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                blood.position.X += (float)Main.rand.Next(-20, 21);
                blood.position.Y += (float)Main.rand.Next(-20, 21);
                blood.velocity *= 0.9f;
                blood.noGravity = true;
                blood.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                blood.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
                if (Main.rand.NextBool())
                    blood.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
            }
        }
    }
}
