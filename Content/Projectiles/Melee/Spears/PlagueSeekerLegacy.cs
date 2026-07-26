using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Spears
{
    public class PlagueSeekerLegacy : CIMeleeProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
        }

        // Reduce damage of projectiles if more than the cap are active
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            int cap = 3;
            float capDamageFactor = 0.05f;
            int excessCount = Main.player[Projectile.owner].ownedProjectileCounts[Type] - cap;
            modifiers.SourceDamage *= MathHelper.Clamp(1f - (capDamageFactor * excessCount), 0f, 1f);
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 150;

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int plagued = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 0.75f);
                    Main.dust[plagued].noGravity = true;
                    Main.dust[plagued].velocity *= 0f;
                }
            }

            if (Projectile.timeLeft < 150)
                LAPUtilities.HomeInNPC(Projectile, 600f, 12f, 20f, null, !Projectile.tileCollide);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CIPlague>(), 180);
        }
    }
}
