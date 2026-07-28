using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Particles;
using CalamityInheritance.Core.Path;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.Cannon
{
    public class SulphuricBlast : BaseStickyProj, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RangedProj;
        public const int TotalSecondsToStick = 8;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 5;
            Projectile.alpha = 255;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 30;
        }
        public override void ExAI()
        {
            if (Projectile.FinalExtraUpdate())
                Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 4 % Main.projFrames[Projectile.type];
            if (!isSticky)
            {
                LAPUtilities.HomeInNPC(Projectile, 1500f, 16f, Projectile.MaxUpdates * 20f);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                if (Main.netMode != NetmodeID.Server && Projectile.FinalExtraUpdate() && Projectile.velocity.Length() > 3f)
                {
                    Color color = new Color(136, 211, 113, 127);
                    Color fadeColor = new Color(165, 165, 86);
                    Vector2 gasSpawnPosition = Projectile.Center + Main.rand.NextVector2Circular(8f, 8f);
                    Vector2 gasVelocity = Projectile.velocity * 1.2f + Projectile.velocity.RotatedBy(0.75f) * 0.3f;
                    gasVelocity *= Main.rand.NextFloat(0.24f, 0.6f);

                    MediumMistParticle gas = new MediumMistParticle(gasSpawnPosition, gasVelocity, color, fadeColor, Main.rand.NextFloat(0.5f, 1f), 205 - Main.rand.Next(50), 0.02f);
                    gas.Spawn();
                }

            }
        }
        public override void ExOnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIIrradiated>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rec = texture2D13.Frame(1, 20, 0, Projectile.frame);
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rec, Color.White, Projectile.rotation, rec.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
