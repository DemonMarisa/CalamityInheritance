using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Sounds.Cals;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using LAP.Core.GlobalInstance.Players.DashSystem;
using LAP.Core.MiscDate;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class AsgardianAegisDashold : BasePlayerDash
    {
        public static int LegacyAsgardianAegisDashDamage = 2000;
        public static float LegacyAsgardianAegisDashKnockback = 15f;
        public int Time;
        public override int ImmuneTime(Player player) => 18;
        public override int DashTime(Player player) => 18;
        public override int DashDelay(Player player) => 20;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(LegacyAsgardianAegisDashDamage, LegacyAsgardianAegisDashKnockback, DamageClass.Melee);
        public override float DashSpeed(Player player) => 24f;
        public override float DashEndSpeedMult(Player player) => 0.5f;
        public override void OnDashStart(Player player)
        {
            // Spawn fire dust around the player's body.
            for (int d = 0; d < 60; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GoldCoin, 0f, 0f, 100, default, 3f);
                holyFireDashDust.position += Main.rand.NextVector2Square(-5f, 5f);
                holyFireDashDust.velocity += Main.rand.NextVector2Circular(5f, 5f);
                holyFireDashDust.velocity *= 0.75f;
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                holyFireDashDust.fadeIn = 0.5f;
            }
            Time = 0;
        }
        public override void DuringDash(Player player)
        {
            Time += 2;
            float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(2f, 2.5f, Time, true));
            for (int d = 0; d < 8; d++)
            {
                int dashDustID = Main.rand.Next(new int[]
                {
                    LAPDustID.DustDungeonSpirit180,
                    LAPDustID.DustShadowBoltStaff173,
                    LAPDustID.DustCopperCoin
                });
                Dust fireDashDust = Dust.NewDustDirect(player.position + Vector2.UnitY * 4f, player.width, player.height - 8, dashDustID, 0f, 0f, 100, default, 2f);
                fireDashDust.velocity *= 0.1f;
                fireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                fireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                fireDashDust.noGravity = true;

                float offsetRotationAngle = player.velocity.ToRotation() + Time / 5f;
                float radius = (15f + (float)Math.Cos(Time / 3f) * 12f) * radiusFactor;
                Vector2 dustPosition = player.Center - player.velocity * 2;
                dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(d / 5f * MathHelper.TwoPi) * radius;
                Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool(5) ? 181 : 295);
                dust.alpha = 220;
                dust.noGravity = true;
                dust.velocity = player.velocity * 0.8f;
                dust.scale = Main.rand.NextFloat(1.7f, 2.0f);
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                Dust dust2 = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) + (player.velocity * 1.5f), Main.rand.NextBool(8) ? 180 : 295, -player.velocity.RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(1.7f, 1.9f));
                dust2.alpha = 170;
                dust2.noGravity = true;
                dust2.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);

                if (Main.rand.NextBool(2))
                    fireDashDust.fadeIn = 0.5f;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
            SoundStyle style = CICalSounds.DevourerSegmentBreak with
            {
                PitchVariance = 0.3f
            };
            SoundEngine.PlaySound(in style, player.Center);
            style = SoundID.Item62 with
            {
                Volume = 0.5f,
                PitchVariance = 0.3f
            };
            SoundEngine.PlaySound(in style, player.Center);
            for (int i = 0; i < 35; i++)
            {
                Dust dust = Dust.NewDustPerfect(player.Center, DustID.GiantCursedSkullBolt, new Vector2(4.5f, 4.5f).RotatedByRandom(100.0) * Main.rand.NextFloat(0.2f, 1.9f), 0, default, Main.rand.NextFloat(1.5f, 2.8f));
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                dust.noGravity = true;
            }
            for (int j = 0; j < 14; j++)
            {
                Vector2 vector = new Vector2(6f, 6f).RotatedByRandom(100.0) * Main.rand.NextFloat(0.5f, 1.2f);
                Dust dust2 = Dust.NewDustPerfect(player.Center + vector * 2f, DustID.WitherLightning, vector);
                dust2.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                Dust.NewDustPerfect(player.Center + vector * 2f, DustID.Electric, vector);
                dust2.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
            }
            ModItem item = ItemLoader.GetItem(ItemType<AsgardianAegisold>());
            Projectile.NewProjectile(player.GetSource_ItemUse(item.Item), player.Center, Vector2.Zero, ProjectileType<HolyExplosionold>(), LegacyAsgardianAegisDashDamage, LegacyAsgardianAegisDashKnockback, Main.myPlayer, 1f, 0f);

            target.AddBuff(BuffType<GodSlayerInferno>(), 300);
        }
    }
}
