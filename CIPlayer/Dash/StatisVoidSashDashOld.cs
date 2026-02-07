using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using CalamityMod.Enums;
using CalamityMod.Particles;
using LAP.Core.GlobalInstance.Players.DashSystem;
using LAP.Core.MiscDate;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class StatisVoidSashDashOld : BasePlayerDash
    {
        public override int ImmuneTime(Player player) => 20;
        public override int DashTime(Player player) => 20;
        public override int DashDelay(Player player) => 20;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(0, 0, DamageClass.Melee);
        public override float DashSpeed(Player player) => 28f;
        public override float DashEndSpeedMult(Player player) => 0.33f;
        public int Time;
        public override bool CanHitNPC(Player player, NPC target)
        {
            return false;
        }
        public override void OnDashStart(Player player)
        {
            Time = 0;
        }
        public override void DuringDash(Player player)
        {
            Time += 2;
            float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(2f, 2.5f, Time, true));
            for (int d = 0; d < 3; d++)
            {
                int dashDustID = Main.rand.Next(new int[]
                {
                    LAPDustID.DustShadowBoltStaff173,
                    LAPDustID.DustShadowBoltStaff173,
                    LAPDustID.DustShadowflame
                });
                Dust fireDashDust = Dust.NewDustDirect(player.position + Vector2.UnitY * 4f, player.width, player.height - 8, dashDustID, 0f, 0f, 100, default, 2f);
                fireDashDust.velocity *= 0.1f;
                fireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                fireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                fireDashDust.noGravity = true;

                Dust dust2 = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) + (player.velocity * 1.5f), Main.rand.NextBool(8) ? 180 : 295, -player.velocity.RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(1.7f, 1.9f));
                dust2.alpha = 170;
                dust2.noGravity = true;
                dust2.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);

                if (Main.rand.NextBool(2))
                    fireDashDust.fadeIn = 0.5f;
            }
            if (Main.myPlayer == player.whoAmI && player.GetModPlayer<LAPDashPlayer>().DashTime % 4 == 0)
            {
                int scytheDamage = (int)player.GetBestClassDamage().ApplyTo(500);
                int scythe = Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ProjectileType<CosmicScytheOld>(), scytheDamage, 5f, player.whoAmI);
                Main.projectile[scythe].DamageType = DamageClass.Generic;
                Main.projectile[scythe].usesIDStaticNPCImmunity = true;
                Main.projectile[scythe].idStaticNPCHitCooldown = 10;
            }
        }
    }
}
