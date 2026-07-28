using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class ProfanedPartisanSpear : CIRogueProj
    {
        public int timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 600;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] != 1f)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.ai[1] = 1f;
                Projectile.ai[0] = 1f;
                Projectile.extraUpdates = 2;
                if (Projectile.timeLeft > 280)
                    Projectile.timeLeft = 280;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CIHolyFlames>(), 180);
            OnHitEffects();
        }
        private void OnHitEffects()
        {
            if (Projectile.ai[1] != 1f)
            {
                Projectile.velocity.X *= -1f;
                Projectile.velocity.Y *= -1f;
                Projectile.ai[1] = 1f;
                Projectile.ai[0] = 1f;
                Projectile.extraUpdates = 2;
                if (Projectile.timeLeft > 280)
                    Projectile.timeLeft = 280;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.ai[0] == 1f)
                timer++;
            if (timer >= 5)
                Projectile.penetrate = 1;
            if (timer >= 10)
            {
                LAPUtilities.HomeInNPC(Projectile, 300f, 7f, 20f, null, !Projectile.tileCollide);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
