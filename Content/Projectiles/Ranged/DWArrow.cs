using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class DWArrow : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
        }

        public override void AI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = (Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi)) + MathHelper.ToRadians(90) * Projectile.direction;

            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
            }

            Lighting.AddLight(Projectile.Center, 0.4f, 0.2f, 0.4f);
            for (int i = 0; i < 2; i++)
            {
                Dust dusty = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool(3) ? 56 : 242, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f)];
                dusty.velocity = Vector2.Zero;
                dusty.position -= Projectile.velocity / 5f * (float)i;
                dusty.noGravity = true;
                dusty.scale = 0.8f;
                dusty.noLight = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<GodSlayerInferno>(), 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<GodSlayerInferno>(), 120);

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[1] == 0f)
                Projectile.localAI[1] = Main.rand.Next(1, 3);
            ModProjectile DWArrow = ProjectileLoader.GetProjectile(ModContent.ProjectileType<DWArrow>());
            switch (Projectile.localAI[1])
            {
                case 2f:
                    texture = ModContent.Request<Texture2D>($"{DWArrow.Texture}" + 2).Value;
                    break;
                default:
                    texture = ModContent.Request<Texture2D>($"{DWArrow.Texture}").Value;
                    break;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
