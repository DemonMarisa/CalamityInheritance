using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class DragonSpearProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.timeLeft = 300;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.localAI[0] > 10f;
        public override void AI()
        {
            Projectile.localAI[0] += 2f;
            Projectile.frame = CIFunction.FramesChanger(Projectile, 6, 4);
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.785f;
			CIFunction.HomeInOnNPC(Projectile, false, 800f, 25f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 160;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
             
            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 30; j++)
            {
                int dAlt = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 1.7f);
                Main.dust[dAlt].noGravity = true;
                Main.dust[dAlt].velocity *= 5f;
                dAlt = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 1f);
                Main.dust[dAlt].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 360);
            for (int j = 0; j < 3; j++)
            {
                Vector2 fireBallSpeed = new Vector2(0 , -26f).RotatedBy(Main.rand.NextFloat(-0.6f + j/10, 0.7f + j/10)) * 1.1f; Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, fireBallSpeed, ModContent.ProjectileType<DragonSpearFlare>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
           
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor , 1);
            return false;
        }
    }
}
