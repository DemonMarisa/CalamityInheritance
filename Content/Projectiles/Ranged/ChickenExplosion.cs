using System;
using System.Runtime.Intrinsics.Arm;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ChickenNukeExplosion: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = ChickenRound.ExplosionHitboxW;
            Projectile.height= ChickenRound.ExplosionHitboxH;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.DamageType = DamageClass.Ranged; 
            Projectile.usesLocalNPCImmunity = true;
            //是的，就是1帧
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.75f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.01f / 255f);
            //造粒子，AI类似于极乐炮的爆炸
            SpawnDust(CIDustID.DustCopperCoin);
            if (Projectile.localAI[0] < 1f)
            {
                SoundEngine.PlaySound(CISoundID.SoundGrenadeExplosion, Projectile.Center);
                Projectile.localAI[0] = 1f;
            }
        }
        
        private void SpawnDust(int dustCopperCoin)
        {
            float maxDustCount = 25f;
            if (Projectile.ai[0] > 180f)
                maxDustCount -= (Projectile.ai[0] - 180f)/2f;
            if (maxDustCount <= 0f) Projectile.Kill();
            maxDustCount *= 0.7f;
            Projectile.ai[0] += 4f;
            int dustCount = 0; 

            while (dustCount < maxDustCount)
            {
                float rx = Main.rand.Next(-120, 121);
                float ry = Main.rand.Next(-120, 121);
                float r3 = Main.rand.Next(36, 108);
                float rDist = CIFunction.TryGetVectorMud(rx, ry);
                rDist = r3 / rDist;
                rx *= rDist;
                ry *= rDist;
                int d = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, dustCopperCoin, 0f, 0f, 100, default, 2.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].position.X = Projectile.Center.X;
                Main.dust[d].position.Y = Projectile.Center.Y;
                Dust what = Main.dust[d];
                what.position.X += Main.rand.Next(-10, 11);
                Dust how = Main.dust[d];
                how.position.Y += Main.rand.Next(-10, 11);
                Main.dust[d].velocity.X = rx;
                Main.dust[d].velocity.Y = ry;
                dustCount++;
            }
        }
    }
}