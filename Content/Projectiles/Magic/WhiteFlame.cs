using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class WhiteFlameLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";

        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            
            Player projOwner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
            for (int i = 0; i < 5; i++)
            {
                int dWhite = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, default, 1.2f);
                Main.dust[dWhite].noGravity = true;
                Main.dust[dWhite].velocity *= 0.5f;
                Main.dust[dWhite].velocity += Projectile.velocity * 0.1f;
            }
            float homingRange = 1000f;
            Projectile.localAI[0] += 1f;
            if(projOwner.active && !projOwner.dead)
            {
                if(Projectile.Distance(projOwner.Center) > homingRange)
                {
                    Projectile.SafeDirectionTo(projOwner.Center, Vector2.UnitX);
                    return;
                }
                if(Projectile.localAI[0] > 10f)
                CIFunction.HomeInOnNPC(Projectile, true, 700f, 16f, 20f);
            }
            else
            {
                if(Projectile.timeLeft > 30)
                   Projectile.timeLeft = 30;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
            int summonCounts = 2;
            if (Projectile.owner == Main.myPlayer)
            {
                for (int j = 0; j < summonCounts; j++)
                {
                    Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    while (speed.X == 0f && speed.Y == 0f)
                    {
                        speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    }
                    speed.Normalize();
                    speed *= Main.rand.Next(70, 101) * 0.1f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.oldPosition.X + Projectile.width / 2, Projectile.oldPosition.Y + Projectile.height / 2, speed.X, speed.Y, ModContent.ProjectileType<WhiteFlameAltLegacy>(), (int)(double)Projectile.damage, 0f, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 360);
        }
    }
}
