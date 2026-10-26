using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Legendary
{
    public abstract class LegendaryProj : ModProjectile, ILocalizedModType
    {
        public override string LocalizationCategory => "Content.Projectiles.LegendaryProj";
        public Player Owner => Main.player[Projectile.owner];
        public SpriteBatch SB { get => Main.spriteBatch; }
        public GraphicsDevice GD { get => Main.graphics.GraphicsDevice; }
    }
    public class BrinyBaronLegendRazor : LegendaryProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.alpha -= 5;
            if (Projectile.alpha < 50)
            {
                Projectile.alpha = 50;
            }

            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 187, 0f, 0f, 100, new Color(53, Main.DiscoG, 255), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 0f;
                }
            }
            Projectile.HomeInNPC(300, 12, 20, 0, ignoreTile: !Projectile.tileCollide);
            base.AI();
        }
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 187, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, new Color(53, Main.DiscoG, 255));
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(53, Main.DiscoG, 255, Projectile.alpha);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor);
            return false;
        }
    }
}
