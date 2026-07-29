using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Rogue.Throws;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Throw
{
    public class StarofDestructionLegacyProj : CIRogueProj
    {
        public override string Texture => GetInstance<StarofDestructionLegacy>().Texture;
        public int hitCount = 0;
        private static float Radius = 47f;

        public override void SetDefaults()
        {
            Projectile.width = 94;
            Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.penetrate = 16;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.98f;
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.SpookyWood, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathHelper.ToRadians(8f);
            if (Projectile.CI().Stealth || hitCount > 16)
                hitCount = 16;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            hitCount++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => LAPUtilities.CircularHitboxCollision(Projectile.Center, Radius, targetHitbox);

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Vector2 vector2 = new Vector2(20f, 20f);
            for (int index1 = 0; index1 < 10; ++index1)
            {
                int index2 = Dust.NewDust(Projectile.Center - vector2 / 2f, (int)vector2.X, (int)vector2.Y, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Dust dust = Main.dust[index2];
                dust.velocity = dust.velocity * 1.4f;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (hitCount < 4)
                    hitCount = 4;
                for (int i = 0; i < hitCount; i++)
                {
                    Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<DestructionBoltLegacy>(), (int)(Projectile.damage * 0.5), 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
    }
}