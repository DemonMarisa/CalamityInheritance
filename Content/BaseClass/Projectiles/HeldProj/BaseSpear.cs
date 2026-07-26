using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles.HeldProj
{
    public abstract class BaseSpear : ModProjectile, ILocalizedModType
    {
        public virtual float RangeMin => 16f;
        public virtual float RangeMax => 106f;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = GetInstance<TrueMelee>();
            Projectile.aiStyle = ProjAIStyleID.Spear;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            int dura = owner.itemAnimationMax;
            owner.heldProj = Projectile.whoAmI;

            //必要时刻重置生命
            if (Projectile.timeLeft > dura)
                Projectile.timeLeft = dura;

            Projectile.velocity = Vector2.Normalize(Projectile.velocity * 5);

            float halfDura = dura * 0.5f;
            float progression;

            if (Projectile.timeLeft < halfDura)
                progression = Projectile.timeLeft / halfDura;
            else
                progression = (dura - Projectile.timeLeft) / halfDura;

            //让矛开始移动
            Projectile.Center = owner.MountedCenter + Vector2.SmoothStep(Projectile.velocity * RangeMin, Projectile.velocity * RangeMax, progression);

            Vector2 rrp = owner.RotatedRelativePoint(owner.MountedCenter, true);
            UpdateAim(rrp, owner.HeldItem.shootSpeed);

            Projectile.rotation = Projectile.velocity.ToRotation();
            ExAI();
        }
        public virtual void ExAI()
        {

        }
        public virtual void UpdateAim(Vector2 source, float speed)
        {
            // Get the player's current aiming direction as a normalized vector.
            Vector2 aim = Vector2.Normalize(Owner.LocalMouseWorld() - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // Change a portion of the Prism's current velocity so that it points to the mouse. This gives smooth movement over time.
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 0.06f));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>(Texture).Value;
            float offset = MathHelper.PiOver4 + (Projectile.spriteDirection == 1 ? 0 : -MathHelper.PiOver2);
            Vector2 offset2 = new Vector2(-texture.Width / 2, 0).RotatedBy(Projectile.rotation);
            Projectile.BaseProjPreDraw(texture, lightColor, offset2, offset, 1);
            return false;
        }
    }
}
