using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class AquaticBeam : CIMeleeProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void AI()
        {
            Projectile.frame = CIUtils.FramesChanger(Projectile, 24, 4);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 45f)
                Projectile.tileCollide = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                Projectile.Kill();
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
                SoundEngine.PlaySound(SoundID.DrumTomLow, Projectile.Center);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 30f)
            {
                Projectile.velocity.X = -Projectile.velocity.X;
                Projectile.velocity.Y = -Projectile.velocity.Y;
                Projectile.localAI[0] = 0f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DrumTomLow, Projectile.Center);
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.oldVelocity.X * 0.25f, Projectile.oldVelocity.Y * 0.25f);
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.SilverCoin, Projectile.oldVelocity.X * 0.25f, Projectile.oldVelocity.Y * 0.25f);
            }
        }
    }
}