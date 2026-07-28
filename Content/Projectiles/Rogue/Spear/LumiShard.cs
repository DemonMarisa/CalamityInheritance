using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class LumiShard : CIRogueProj
    {
        bool canGrav = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 18;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target) => target.CanBeChasedBy(Projectile, false);
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(canGrav);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            canGrav = reader.ReadBoolean();
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0f && Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                canGrav = true;

            Projectile.ai[0] = 1f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

            if (canGrav)
                Projectile.velocity.Y *= 1.05f;

            if (Projectile.timeLeft < 90)
            {
                LAPUtilities.HomeInNPC(Projectile, 1000f, 16f, 20f);
            }

            int num309 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.BubbleBurst_Blue, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
            Main.dust[num309].velocity *= -0.25f;
            num309 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.BubbleBurst_Pink, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
            Main.dust[num309].velocity *= -0.25f;
            Main.dust[num309].position -= Projectile.velocity * 0.5f;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISounds.LumiShardHit, Projectile.Center);
            for (int i = 0; i <= 2; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, Projectile.oldVelocity.X / 4, Projectile.oldVelocity.Y / 4, 0, default, 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;

                d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BubbleBurst_Pink, Projectile.oldVelocity.X / 4, Projectile.oldVelocity.Y / 4, 0, default, 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

    }
}