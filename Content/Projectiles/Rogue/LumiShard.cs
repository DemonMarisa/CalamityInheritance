using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class LumiShard: ModProjectile, ILocalizedModType
    {
        bool canGrav = false;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height= 18;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }
        public override bool? CanHitNPC(NPC target) => target.CanBeChasedBy(Projectile, false);
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
                if (Projectile.ai[1] != 1f)
                    CIFunction.HomeInOnNPC(Projectile, true, 1000f, 16f, 20f);
                else
                    //潜伏攻击的月明碎片的索敌距离更短——这个是故意为之
                    CIFunction.HomeInOnNPC(Projectile, true, 900f, 14f ,15f);
            }

            int num309 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.BubbleBurst_Blue, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
            Main.dust[num309].velocity *= -0.25f;
            num309 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, DustID.BubbleBurst_Pink, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(0, 255, 255), 0.5f);
            Main.dust[num309].velocity *= -0.25f;
            Main.dust[num309].position -= Projectile.velocity * 0.5f;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISoundMenu.LumiShardHit, Projectile.Center);
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
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

    }
}