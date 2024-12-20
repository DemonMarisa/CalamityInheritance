﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Summon;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class LeechingDaggerProj : BaseShortswordProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(13);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>(); ;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 26 / 2;
            const int HalfSpriteHeight = 26 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
            {
                int Demonite = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Demonite, (float)(Projectile.direction * 2), 0f, 15, default, 1.3f);
            }
        }
        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 6f, ModContent.ProjectileType<EyeOfNightCell>(), (int)(Projectile.damage * 1), Projectile.knockBack, Projectile.owner, 0f, 0f);
        };

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = Projectile.GetSource_FromThis();
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<DarkBall>(), Projectile.damage * 2, Projectile.knockBack);
            target.AddBuff(BuffID.CursedInferno, 300);
        }
    }
}
