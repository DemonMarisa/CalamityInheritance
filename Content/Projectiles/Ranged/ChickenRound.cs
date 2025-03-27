using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ChickenRound: ModProjectile, ILocalizedModType
    {
        public static readonly int MaxFrames = 4;
        public static readonly int FramesCounter = 4;
        public static readonly int ExplosionHitboxW = 1040;
        public static readonly int ExplosionHitboxH = 1040;
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.frame = CIFunction.FramesChanger(Projectile, FramesCounter, MaxFrames);
            if (Math.Abs(Projectile.velocity.X) > 7f || Math.Abs(Projectile.velocity.Y) > 7f)
            {
                SpawnsDust(CIDustID.DustCopperCoin,CIDustID.DustTorchNormal);
                
                if (Math.Abs(Projectile.velocity.X) < 15f && Math.Abs(Projectile.velocity.Y) < 15f)
                    Projectile.velocity *= 1.1f;

                else if (Main.rand.NextBool(2))
                SpawnsDust2(CIDustID.DustCopperCoin,CIDustID.DustTorchNormal);
            }
            Projectile.ai[0] += 1f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f; 
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0f;
            Projectile.velocity.Y = -15f;
            Projectile.timeLeft = 20;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChickenNukeExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Vector2 dPos = new(Projectile.Center.X - (ExplosionHitboxW / 2), Projectile.Center.Y - (ExplosionHitboxH / 2));
            OnKillDust(CIDustID.DustSmoke, CIDustID.DustTorchNormal, dPos);
        }

        private void OnKillDust(int dustSmoke, int dustTorchNormal, Vector2 dPos)
        {
            for (int i = 0; i < 40; i++)
            {
                int dGet = Dust.NewDust(dPos, Projectile.width, Projectile.height, dustSmoke, 0f, 0f, 100, default, 2f);
                Main.dust[dGet].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[dGet].scale = 0.5f;
                    Main.dust[dGet].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 70; j++)
            {
                int dAlt = Dust.NewDust(dPos, Projectile.width, Projectile.height, dustTorchNormal, 0f, 0f, 100, default, 3f);
                Main.dust[dAlt].noGravity = true;
                Main.dust[dAlt].velocity *= 5f;
                dAlt = Dust.NewDust(dPos, Projectile.width, Projectile.height, dustTorchNormal, 0f, 0f, 100, default, 2f);
                Main.dust[dAlt].velocity *= 2f;
            }
        }

        private void SpawnsDust2(int dustCopperCoin, int dustTorchNormal)
        {
            Vector2 offset = new Vector2(0f, (float)(-(float)Projectile.height/2)).RotatedBy(Projectile.rotation, default) * 1.1f;
            Vector2 offset2 = new Vector2(0f, (float)(-(float)Projectile.height/2 - 6)).RotatedBy(Projectile.rotation, default) * 1.1f;
            int dGet = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, dustCopperCoin, 0f, 0f, 100, default, 1f);
            Main.dust[dGet].scale = 0.1f + Main.rand.Next(5) * 0.1f;
            Main.dust[dGet].fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
            Main.dust[dGet].noGravity = true;
            Main.dust[dGet].position = Projectile.Center + offset;
            dGet = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, dustTorchNormal, 0f, 0f, 100, default, 1f);
            Main.dust[dGet].scale = 1f + Main.rand.Next(5) * 0.1f;
            Main.dust[dGet].noGravity = true;
            Main.dust[dGet].position = Projectile.Center + offset2;
        }

        public void SpawnsDust(int dType, int dType2)
        {
            for (int i = 0; i < 2 ; i++)
            {
                float dVelX = i > 0 ? Projectile.velocity.X * 0.5f : 0f;
                float dVelY = i > 0 ? Projectile.velocity.Y * 0.5f : 0f;

                int dGet = Dust.NewDust(new Vector2(Projectile.position.X + 3f + dVelX, Projectile.position.Y + 3f + dVelY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, dType2, 0f, 0f, 100, default, 1f);
                Main.dust[dGet].scale *= 2f + Main.rand.Next(10) * 0.1f;
                Main.dust[dGet].noGravity = true;
                dGet = Dust.NewDust(new Vector2(Projectile.position.X + 3f + dVelX, Projectile.position.Y + 3f + dVelY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, dType, 0f, 0f, 100, default, 0.5f);
                Main.dust[dGet].fadeIn = 1f + Main.rand.Next(5) * 0.1f;
                Main.dust[dGet].velocity *= 0.05f;
            }
        }
    }
}