﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class CosmicShivProjold : BaseShortswordProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(24);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }

        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 12f, ModContent.ProjectileType<CosmicShivBallold>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
        };

        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 48 / 2;
            const int HalfSpriteHeight = 48 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public override void ExtraBehavior()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 173);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int k = 0; k < 36; k++)
            {
                int dustID = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height - 16, 173, 0f, 0f, 0, default, 1f);
                Main.dust[dustID].velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int k = 0; k < 36; k++)
            {
                int dustID = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height - 16, 173, 0f, 0f, 0, default, 1f);
                Main.dust[dustID].velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);
        }
    }
}
