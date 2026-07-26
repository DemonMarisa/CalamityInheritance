using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class CrescentMoonProjLegacy : CIMeleeProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.alpha = 100;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 140;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 2;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 13;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.6f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 30 + Main.rand.Next(50);
                if (Main.rand.NextBool(10))
                {
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            Projectile.rotation += Projectile.direction * 0.55f;
            Projectile.HomeInNPC(1800f, 18f, 20f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CINightwither>(), 180);
        }
    }
}
