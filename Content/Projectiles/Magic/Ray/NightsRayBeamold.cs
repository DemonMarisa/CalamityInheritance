using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class NightsRayBeamold : ModProjectile, ILocalizedModType
    {
        public const int Lifetime = 200;

        public const float MaxExponentialDamageBoost = 3f;

        public static readonly float ExponentialDamageBoost = (float)Math.Pow(MaxExponentialDamageBoost, 1f / Lifetime);
        public ref float InitialDamage => ref Projectile.ai[1];
        public new string LocalizationCategory => "Projectiles.Magic";
        public ref float Time => ref Projectile.ai[0];
        public bool HasFiredSideBeams
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value.ToInt();
        }
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
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
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<NightOrbold>(), (int)(Projectile.damage * 0.6), (int)Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            if (InitialDamage == 0f)
            {
                InitialDamage = Projectile.damage;
                Projectile.netUpdate = true;
            }

            Time++;
            Projectile.damage = (int)(InitialDamage / Math.Pow(ExponentialDamageBoost, Time));

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustSpawnPos = Projectile.position - Projectile.velocity * i / 2f;
                    Dust corruptMagic = Dust.NewDustPerfect(dustSpawnPos, 27);
                    corruptMagic.color = Color.Lerp(Color.Fuchsia, Color.Magenta, Main.rand.NextFloat(0.6f));
                    corruptMagic.scale = Main.rand.NextFloat(0.96f, 1.04f);
                    corruptMagic.noGravity = true;
                    corruptMagic.velocity *= 0.1f;
                }
            }
        }
    }
}
