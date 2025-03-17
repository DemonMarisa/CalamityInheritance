using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.NPCs.Calamitas.Projectiles
{
    public class BrimstoneLaser : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";
        private int splitTimer = 45;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brimstone Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.scale = 2f;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 120;
            AIType = ProjectileID.DeathLaser;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(splitTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            splitTimer = reader.ReadInt32();
        }

        public override void AI()
        {
            splitTimer--;
            if (splitTimer ==5)
                CIFunction.DustCircle(Projectile.Center, 16, 1f, DustID.CrimsonTorch, false, 8f);
            if (splitTimer <= 0)
            {
                int numProj = 2;
                float rotation = MathHelper.ToRadians(20);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < numProj + 1; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<BrimstoneLaserSplit>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                    }
                }
                Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.05f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);
            Projectile.velocity.X *= 1.01f;
            Projectile.velocity.Y *= 1.01f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 50, 50, Projectile.alpha);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
