using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class VanquisherArrowSplit : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Items/Ammo/RangedAmmo/VanquisherArrowold";

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.arrow = true;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 600f, 12f, 20f);
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.timeLeft < 90)
            {
                Vector2 origin = new Vector2(0f, 0f);
                Color color = Color.White;
                if (Projectile.timeLeft < 85)
                {
                    byte b2 = (byte)(Projectile.timeLeft * 3);
                    byte a2 = (byte)(100f * (b2 / 255f));
                    color = new Color(b2, b2, b2, a2);
                }
                Texture2D baseTexture = Request<Texture2D>(Texture).Value;
                Rectangle frame = new Rectangle(0, 0, baseTexture.Width, baseTexture.Height);
                Main.EntitySpriteDraw(Request<Texture2D>("CalamityInheritance/Content/Items/Ammo/RangedAmmo/VanquisherArrowoldGlow").Value, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(0, 0, 0, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<GodSlayerInferno>(), 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<GodSlayerInferno>(), 120);
    }
}
