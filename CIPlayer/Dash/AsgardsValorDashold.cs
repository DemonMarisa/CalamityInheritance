﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Enums;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Content.Items.Accessories;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class AsgardsValorDashold : CIPlayerDashEffect
    {
        public static new string ID => "Asgard's Valorold";

        public override DashCollisionType CollisionType => DashCollisionType.ShieldSlam;

        public override bool IsOmnidirectional => false;

        public override float CalculateDashSpeed(Player player) => 16.9f;

        public override void OnDashEffects(Player player)
        {
            // Spawn fire dust around the player's body.
            for (int d = 0; d < 20; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position, player.width, player.height, 246, 0f, 0f, 100, default, 3f);
                holyFireDashDust.position += Main.rand.NextVector2Square(-5f, 5f);
                holyFireDashDust.velocity *= 0.2f;
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                holyFireDashDust.fadeIn = 0.5f;
            }
        }

        public override void MidDashEffects(Player player, ref float dashSpeed, ref float dashSpeedDecelerationFactor, ref float runSpeedDecelerationFactor)
        {
            // Spawn fire dust around the player's body.
            for (int d = 0; d < 4; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position + Vector2.UnitY * 4f, player.width, player.height - 8, 246, 0f, 0f, 100, default, 2.75f);
                holyFireDashDust.velocity *= 0.1f;
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                if (Main.rand.NextBool(2))
                    holyFireDashDust.fadeIn = 0.5f;
            }
        }

        public override void OnHitEffects(Player player, NPC npc, IEntitySource source, ref CIDashHitContext hitContext)
        {
            float kbFactor = 9f;
            bool crit = Main.rand.Next(100) < player.GetCritChance<MeleeDamageClass>();
            if (player.kbGlove)
                kbFactor *= 2f;
            if (player.kbBuff)
                kbFactor *= 1.5f;

            int hitDirection = player.direction;
            if (player.velocity.X != 0f)
                hitDirection = Math.Sign(player.velocity.X);

            // Define hit context variables.
            hitContext.HitDirection = hitDirection;
            hitContext.BaseKnockback = kbFactor;
            hitContext.PlayerImmunityFrames = AsgardsValor.ShieldSlamIFrames;
            hitContext.damageClass = DamageClass.Melee;

            int holyExplosionDamage = (int)player.GetBestClassDamage().ApplyTo(60);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<HolyExplosion>(), holyExplosionDamage, 15f, Main.myPlayer, 0f, 0f);
            npc.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
    }
}
