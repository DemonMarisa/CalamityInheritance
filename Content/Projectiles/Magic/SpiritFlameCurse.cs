using CalamityInheritance.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class SpiritFlameCurse : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.scale *= 1.3f;
            Projectile.DamageType = DamageClass.Magic;
            //给他两穿透让他能造成两次伤害
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
			Projectile.coldDamage = true;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.985f;
            Projectile.velocity.Y *= 0.985f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 7)
            {
                Projectile.frame = 0;
            }
            Lighting.AddLight(Projectile.Center, 0f, 0.25f, 0.5f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(35,185,255);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 160;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            SoundEngine.PlaySound(CISoundID.SoundGrenadeExplosion, Projectile.position);
            //这里的粒子压制在了一个非常非常低的值
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 160, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 5; j++)
            {
                int dustAlt = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 160, 0f, 0f, 100, default, 3f);
                Main.dust[dustAlt].noGravity = true;
                Main.dust[dustAlt].velocity *= 5f;
                dustAlt = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 160, 0f, 0f, 100, default, 2f);
                Main.dust[dustAlt].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 180);
        }
    }
}
