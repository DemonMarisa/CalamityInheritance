﻿using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class MidnightSunBeaconProjold : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Summon";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Summon/MidnightSunBeaconold";
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
            Projectile.tileCollide = true;
            Projectile.timeLeft = 420;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.rotation.AngleLerp(-MathHelper.PiOver4, 0.03f);
            if (Math.Abs(Projectile.rotation + MathHelper.PiOver4) < 0.02f && Projectile.ai[0] == 0f)
            {
                for (int i = 1; i <= 4; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MidnightSunSkyBeamold>(), Projectile.damage, Projectile.knockBack, Projectile.owner,
                        Projectile.whoAmI, i - 2);
                }
                for (int i = 1; i <= 4; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MidnightSunSkyBeamolddownup>(), Projectile.damage, Projectile.knockBack, Projectile.owner,
                        Projectile.whoAmI, i - 2);
                }
                Projectile.ai[1] = MidnightSunSkyBeamold.TrueTimeLeft + 60f;
                Projectile.ai[0] = 1f;
            }
            if (Projectile.ai[1] == 1f)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * 30f, ModContent.ProjectileType<MidnightSunUFOold>(), Projectile.damage, Projectile.knockBack,Projectile.owner);
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