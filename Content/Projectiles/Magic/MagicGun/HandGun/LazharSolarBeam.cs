using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Projectiles.Typeless.Explosions;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.MagicGun.HandGun
{
    public class LazharSolarBeam : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 projPos = Projectile.position;
                    projPos -= Projectile.velocity * (i * 0.25f);
                    Projectile.alpha = 255;
                    int heatGold = Dust.NewDust(projPos, 1, 1, DustID.GoldCoin, 0f, 0f, 0, default, 1f);
                    Main.dust[heatGold].position = projPos;
                    Main.dust[heatGold].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[heatGold].velocity *= 0.2f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 180);
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<FuckYou>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                Main.projectile[proj].DamageType = DamageClass.Magic;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Daybreak, 180);
    }
}
