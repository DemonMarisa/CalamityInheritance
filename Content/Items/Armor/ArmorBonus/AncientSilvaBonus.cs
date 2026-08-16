using CalamityInheritance.Core.GlobalInstance.Players;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class AncientSilvaBonus
    {
        public static float regenPool;
        public static void AncientSilvaArmor_LifeRegen(CIPlayer player)
        {
            int green = Dust.NewDust(player.Player.position, player.Player.width, player.Player.height, DustID.ChlorophyteWeapon, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 2f);
            Main.dust[green].position.X += Main.rand.Next(-20, 21);
            Main.dust[green].position.Y += Main.rand.Next(-20, 21);
            Main.dust[green].velocity *= 0.9f;
            Main.dust[green].noGravity = true;
            Main.dust[green].scale *= 1f + Main.rand.Next(40) * 0.01f;
            Main.dust[green].shader = GameShaders.Armor.GetSecondaryShader(player.Player.ArmorSetDye(), player.Player);
            if (Main.rand.NextBool())
                Main.dust[green].scale *= 1f + Main.rand.Next(40) * 0.01f;
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            // 储存具体的回血进度
            // 因为回血的数值1 = 0.5HP/s，所以除120
            int Source = player.Player.lifeRegen;
            if (Source < -20)
                Source = -20;
            float lifeRegenTimer = Math.Abs((float)(Source / 120f));
            regenPool += lifeRegenTimer;
            if (regenPool > 1f)
            {
                player.Player.NCHeal((int)regenPool);
                regenPool -= (int)regenPool;
            }

            if (player.Player.miscCounter % 15 == 0)
                player.Player.NCHeal(1);

            if (regenPool < 0f)
                regenPool = 0f;
        }
        public static void AncientSilvaArmorFastRegen_LifeRegen(CIPlayer player)
        {
            if (player.HasCount("AncientSilvaArmorFastRegen"))
            {
                if (player.CICD["AncientSilvaArmorFastRegen"] < 2700)
                    return;
            }
            for (int i = 0; i < 15; i++)
            {
                if (Main.rand.NextBool())
                {
                    Vector2 offset = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                    Vector2 velOffset = new Vector2(8f, 0).RotatedBy(offset.ToRotation());
                    float dFlyVelX = player.Player.velocity.X * 0.8f + velOffset.X;
                    float dFlyVelY = player.Player.velocity.Y * 0.8f + velOffset.Y;
                    float dScale = 1.2f;
                    Dust dust = Dust.NewDustPerfect(new Vector2(player.Player.Center.X, player.Player.Center.Y) + offset, DustID.GemEmerald, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                    dust.noGravity = true;
                }
                if (Main.rand.NextBool(6))
                {
                    Vector2 offset = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                    Vector2 velOffset = new Vector2(8f, 0).RotatedBy(offset.ToRotation());
                    float dFlyVelX = player.Player.velocity.X * 0.7f + velOffset.X;
                    float dFlyVelY = player.Player.velocity.Y * 0.7f + velOffset.Y;
                    float dScale = 1.2f;
                    Dust dust = Dust.NewDustPerfect(new Vector2(player.Player.Center.X, player.Player.Center.Y) + offset, DustID.Vortex, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                    dust.noGravity = true;
                }
            }
            player.Player.NCHeal(3);
            player.AddCount("AncientSilvaArmorFastRegen", 2700 + 60);
        }
        public static void AncientSilvaArmor_Hurt(Player player,Player.HurtInfo info)
        {
            player.NCHeal(info.Damage / 10);
        }
    }
}
