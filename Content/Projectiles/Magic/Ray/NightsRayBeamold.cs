using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class NightsRayBeamold : CIMagicProj
    {
        public const int Lifetime = 200;

        public ref float InitialDamage => ref Projectile.ai[1];
        public ref float Time => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public bool HasFiredSideBeams
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value.ToInt();
        }
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, fireVel, ProjectileType<NightBoltold>(), (int)(Projectile.damage * 0.6), (int)Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }

            if (InitialDamage == 0f)
            {
                InitialDamage = Projectile.damage * 0.6f;
                Projectile.netUpdate = true;
            }

            float damageboost = (float)(Time / Lifetime) * 3f;

            Projectile.damage = (int)(InitialDamage * (4f - damageboost));

            Time++;
            if (Time > 9f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustSpawnPos = Projectile.Center - Projectile.velocity * i / 2f;
                    Dust corruptMagic = Dust.NewDustPerfect(dustSpawnPos, DustID.Shadowflame);
                    corruptMagic.color = Color.Lerp(Color.Fuchsia, Color.Magenta, Main.rand.NextFloat(0.6f));
                    corruptMagic.scale = Main.rand.NextFloat(0.96f, 1.04f);
                    corruptMagic.noGravity = true;
                    corruptMagic.velocity *= 0.1f;
                }
            }
        }
    }
}
