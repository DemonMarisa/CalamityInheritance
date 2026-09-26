using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.HomeIn
{
    public class PinkFlower : CITypelessProj
    {
        public bool UseThorn => Projectile.ai[0] == 1f;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            if (UseThorn)
                tex = Request<Texture2D>($"{Texture}_Thorn").Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += (MathF.Abs(Projectile.velocity.X) + MathF.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
            Projectile.velocity *= 0.98f;
            for (int dust = 0; dust < 2; dust++)
            {
                int randomDust = Utils.SelectRandom(Main.rand, new int[]
                {
                    164,
                    58,
                    204
                });
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, randomDust, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 6; k++)
            {
                int randomDust = Utils.SelectRandom(Main.rand, new int[]
                {
                    164,
                    58,
                    204
                });
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, randomDust, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 velocity = ((MathHelper.TwoPi * i / 8f) - (MathHelper.ToRadians(67.5f) - Projectile.velocity.ToRotation())).ToRotationVector2() * 4f;
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<PinkPetal>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
                    Main.projectile[p].DamageType = Projectile.DamageType;
                    if (Main.projectile[p].CI().Stealth)
                        Main.projectile[p].SetStealthAttack();
                }
            }
            SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);
        }

    }
}
