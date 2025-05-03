using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class FulgurationHalberdProj : ModProjectile, ILocalizedModType
    {
        protected virtual float RangeMin => 16f;
        protected virtual float RangeMax => 106f;
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.aiStyle = ProjAIStyleID.Spear;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
        }

        public override bool PreAI()
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

            //给矛一个正确的转角
            if (Projectile.spriteDirection == -1)
                //贴图朝左，转45°
                Projectile.rotation += MathHelper.ToRadians(45f);
            else
                //贴图朝右，转135°
                Projectile.rotation += MathHelper.ToRadians(135f);
            if (Projectile.ai[0] == 0f)
            {
                //让矛刺出的第一帧发射弹幕，而非顶点发射
                Projectile.ai[0] = 1f;
            }

            Vector2 rrp = owner.RotatedRelativePoint(owner.MountedCenter, true);
            UpdateAim(rrp, owner.HeldItem.shootSpeed);

            //干掉AI钩子
            return false;
        }

        public virtual void UpdateAim(Vector2 source, float speed)
        {
            // Get the player's current aiming direction as a normalized vector.
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // Change a portion of the Prism's current velocity so that it points to the mouse. This gives smooth movement over time.
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 0.04f));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
        }
    }
}
