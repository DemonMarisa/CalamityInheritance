using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public abstract class ShizukuBaseGhost : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public virtual int ProjWidth { get; }
        public virtual int ProjHeight { get; }
        public virtual float TimeToHoming { get; }
        public ref float AttackTimer => ref Projectile.ai[0];
        public const float MoreTrailingDust = 1f;
        public const float NoMoreTrailingDust = 0f;
        public bool ShouldMoreDust
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value? 1f : 0f;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = ProjWidth;
            Projectile.height = ProjHeight;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false; 
            Projectile.timeLeft = 600;
        }
        public override bool? CanDamage() => AttackTimer > TimeToHoming;
        public override void AI()
        {
            Projectile.FramesChanger(6, 4);
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Lighting.AddLight(Projectile.Center, 0.5f, 0.2f, 0.9f);
            AttackTimer++;
            if (AttackTimer > TimeToHoming)
            {
                if (Main.rand.NextBool(3))
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 12f, 20f);
            }
            if (Main.rand.NextBool(5) || ShouldMoreDust)
            {
                int ghostlyDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFlare, 0f, 0f, 0, Color.SkyBlue, 1f);
                Dust dust = Main.dust[ghostlyDust];
                dust.velocity *= 0.1f;
                Main.dust[ghostlyDust].scale = 1.3f;
                Main.dust[ghostlyDust].noGravity = true;
            }
        }
        public virtual void ExtraAI() {}
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 595)
                return false;

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public virtual void ExtraOnKill() {}
        public override void OnKill(int timeLeft)
        {
            ExtraOnKill();
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 64;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.NPCDeath39 with {MaxInstances = 0}, Projectile.position);
            int dustAmt = 36;
            if (ShouldMoreDust)
            {
                for (int i = 0; i < dustAmt; i++)
                {
                    Vector2 rotate = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                    rotate = rotate.RotatedBy((double)((i - (dustAmt / 2 - 1)) * 6.28318548f / dustAmt), default) + Projectile.Center;
                    Vector2 faceDirection = rotate - Projectile.Center;
                    int killedDust = Dust.NewDust(rotate + faceDirection, 0, 0, DustID.BlueFlare, faceDirection.X * 1.5f, faceDirection.Y * 1.5f, 100, Color.SkyBlue, 2f);
                    Main.dust[killedDust].noGravity = true;
                    Main.dust[killedDust].noLight = true;
                    Main.dust[killedDust].velocity = faceDirection;
                }
            }
        }
    }
}