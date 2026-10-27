using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.CDs;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace CalamityInheritance.Content.Buff.Armor.Melee
{
    public class TarragonCloak : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.CI().ContactDamageReduction *= 0.5f;
            for (int j = 0; j < 2; j++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.ChlorophyteWeapon, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 2f);
                dust.position.X += (float)Main.rand.Next(-20, 21);
                dust.position.Y += (float)Main.rand.Next(-20, 21);
                dust.velocity *= 0.9f;
                dust.noGravity = true;
                dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
                if (Main.rand.NextBool())
                    dust.scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
            }
        }
        public override bool RightClick(int buffIndex)
        {
            Main.LocalPlayer.AddCD(LAPContent.CDType<TarragonCloakCD>(), 1500);
            return true;
        }
    }
}
