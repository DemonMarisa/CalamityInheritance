using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Summon.Normal.Fighter;
using CalamityInheritance.Content.Projectiles.Summon.Normal.Fighter;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.Proj.Fighter
{
    public class MidnightSunBeaconProjold : CISummonProj
    {
        public override string Texture => GetInstance<MidnightSunBeaconold>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 420;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.rotation.AngleLerp(-MathHelper.PiOver4, 0.03f);
            if (Math.Abs(Projectile.rotation + MathHelper.PiOver4) < 0.02f && Projectile.ai[0] == 0f)
            {
                for (int i = 1; i <= 4; i++)
                {
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<MidnightSunSkyBeamold>(), Projectile.damage, Projectile.knockBack, Projectile.owner,
                        Projectile.whoAmI, i - 2);
                    p.originalDamage = Projectile.originalDamage;
                }
                Projectile.ai[1] = MidnightSunSkyBeamold.TrueTimeLeft + 60f;
                Projectile.ai[0] = 1f;
            }
            if (Projectile.ai[1] == 1f)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * 30f, ProjectileType<MidnightSunUFOold>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[p].originalDamage = Projectile.originalDamage;
                Projectile.Kill();
            }
            if (Projectile.ai[1] > 0)
                Projectile.ai[1]--;
            if (Projectile.ai[1] > 1f &&
                Projectile.ai[1] <= 60f)
            {
                Projectile.velocity.Y -= 0.4f;
            }
            else
                Projectile.velocity *= 0.96f;
        }
        public override bool? CanDamage() => false;
    }
}
