using CalamityMod.Buffs.DamageOverTime;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalNer : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            int dType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Main.dust[dType].noGravity = true;
            Main.dust[dType].velocity *= 0f;
            float projX = Projectile.Center.X;
            float projY = Projectile.Center.Y;
            float homingR = 600f;
            bool canHome = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                {
                    float getnpcX = Main.npc[i].position.X + Main.npc[i].width / 2;
                    float getnpcY = Main.npc[i].position.Y + Main.npc[i].height / 2;
                    float getnpcR = Math.Abs(Projectile.position.X + Projectile.width / 2 - getnpcX) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - getnpcY);
                    if (getnpcR < homingR)
                    {
                        homingR = getnpcR;
                        projX = getnpcX;
                        projY = getnpcY;
                        canHome = true;
                    }
                }
            }
            if (canHome)
            {
                float speed = 12f;
                Vector2 projVel = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                float newX = projX - projVel.X;
                float newY = projY - projVel.Y;
                float newRange = (float)Math.Sqrt((double)(newX * newX + newY * newY));
                newRange = speed / newRange;
                newX *= newRange;
                newY *= newRange;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + newX) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + newY) / 21f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 180);
        }
    }
}
