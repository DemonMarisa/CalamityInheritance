using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityMod.Dusts;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class BloodRayold : CIMagicProj
    {
        public const int Lifetime = 200;
        public ref float Time => ref Projectile.ai[0];
        public ref float InitialDamage => ref Projectile.ai[1];
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = Lifetime;
        }
        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 29f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 300, false);
                if (npc is not null)
                {
                    Vector2 fireVel = LAPUtilities.GetVector2(Projectile.Center, npc.Center) * 6f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, fireVel, ProjectileType<BloodBolt>(), (int)(Projectile.damage * 0.6), (int)Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }

            if (InitialDamage == 0f)
            {
                InitialDamage = Projectile.damage;
                Projectile.netUpdate = true;
            }
            float damageboost = (float)(Time / Lifetime) * 2;

            Projectile.damage = (int)(InitialDamage * damageboost);

            Time++;
            if (Time >= 9f)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustType = Main.rand.NextBool(4) ? 182 : DustID.LifeDrain;
                    Vector2 dustSpawnPos = Projectile.Center - Projectile.velocity * i / 2f;
                    Dust crimtameMagic = Dust.NewDustPerfect(dustSpawnPos, dustType);
                    crimtameMagic.scale = Main.rand.NextFloat(0.96f, 1.04f) * MathHelper.Lerp(1f, 1.7f, Time / Lifetime);
                    crimtameMagic.noGravity = true;
                    crimtameMagic.velocity *= 0.1f;
                }
            }
        }
    }
}
