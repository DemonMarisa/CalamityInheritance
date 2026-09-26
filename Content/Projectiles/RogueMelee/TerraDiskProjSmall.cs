using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class TerraDiskProjSmall : CIMeleeRogueProj
    {
        public ref float rotation => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 75;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            StealthStrikeAI();
            LightingandDust();
        }

        private void StealthStrikeAI()
        {

            Projectile.rotation += 0.4f * Projectile.direction;

            Projectile parent = Main.projectile[(int)Projectile.ai[0]];

            if (parent.active && parent.type == ProjectileType<TerraDiskProj>())
            {
                Projectile.timeLeft = 2;

                Vector2 vector = parent.Center - Projectile.Center;
                Projectile.Center = parent.Center + new Vector2(80, 0).RotatedBy(rotation);
                float rotateAmt = 0.1f;
                rotation += rotateAmt;
                if (rotation >= 360)
                {
                    rotation = 0;
                }
                Projectile.velocity.X = vector.X > 0f ? -0.000001f : 0f;
            }
            else
            {
                Projectile.Kill();
            }

            if (!parent.active)
            {
                Projectile.Kill();
            }
        }

        private void LightingandDust()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.75f, 0f);
            if (!Main.rand.NextBool(5))
                return;
            int p = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, 0, 0);
            Main.dust[p].velocity = Vector2.Zero;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}