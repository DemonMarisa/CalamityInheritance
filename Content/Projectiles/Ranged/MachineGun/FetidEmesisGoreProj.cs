using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Ranged.MachineGun
{
    public class FetidEmesisGoreProj : CIRangedProj
    {
        public int HurtCounter = 0;
        public const int HurtTimeIncrement = 10;
        public override void ExSD()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        public override void AI()
        {
            // Override the default DOT used in the method above.
            //复写上方stickproj方法的ai0
            //我十分推荐这里重写，点开stickproj发现简直是依托大的
            if (Projectile.ai[0] == 1f)
            {
                Projectile.localAI[0] = 5f;
                Projectile.velocity = Vector2.Zero;
                HurtCounter++;
                if (HurtCounter % HurtTimeIncrement == 0)
                {
                    Main.npc[(int)Projectile.ai[1]].HitEffect(0, 50.0);
                }
            }
            else
            {
                Projectile.rotation += (Projectile.velocity.X > 0).ToDirectionInt() * MathHelper.ToRadians(8f);
            }
            if (Projectile.timeLeft % 12 == 11)
            {
                for (int i = 0; i < (Projectile.ai[0] == 1f ? 3 : 1); i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, 10, 10, DustID.Shadowflame);
                    dust.velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi);
                    dust.noGravity = true;
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return null;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIIrradiated>(), 60);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIIrradiated>(), 60);
        }
    }
}
