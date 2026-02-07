using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using LAP.Core.GlobalInstance.Players.DashSystem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class AsgardsValorDashold : BasePlayerDash
    {
        public static int LegacyAsgardsValorDashDamage = 150;
        public static float LegacyAsgardsValorDashKnockback = 9f;
        public int Time;
        public override int ImmuneTime(Player player) => 12;
        public override int DashTime(Player player) => 12;
        public override int DashDelay(Player player) => 28;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(LegacyAsgardsValorDashDamage, LegacyAsgardsValorDashKnockback, DamageClass.Melee);
        public override float DashSpeed(Player player) => 20f;
        public override float DashEndSpeedMult(Player player) => 0.5f;
        public override void OnDashStart(Player player)
        {
            for (int d = 0; d < 20; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GoldCoin, 0f, 0f, 100, default, 3f);
                holyFireDashDust.position += Main.rand.NextVector2Square(-5f, 5f);
                holyFireDashDust.velocity *= 0.2f;
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                holyFireDashDust.fadeIn = 0.5f;
            }
        }

        public override void DuringDash(Player player)
        {
            for (int d = 0; d < 10; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position + Vector2.UnitY * 4f, player.width, player.height - 8, Main.rand.NextBool() ? 296 : 158, 0f, 0f, 0, default, 1.2f);
                holyFireDashDust.velocity = -player.velocity * Main.rand.NextFloat(0.1f, 0.75f);
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                if (Main.rand.NextBool())
                    holyFireDashDust.fadeIn = 0.1f;
            }
            if (Main.rand.NextBool(2))
            {
                Vector2 dustPosition = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (player.velocity * 1.7f);
                Dust dust = Dust.NewDustPerfect(dustPosition, DustID.FireworkFountain_Yellow, -player.velocity * Main.rand.NextFloat(0.15f, 0.4f), 0, default, 0.5f);
                dust.noGravity = false;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
            }
        }

        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
            int Dusts = 12;
            float radians = MathHelper.TwoPi / Dusts;
            Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
            for (int k = 0; k < Dusts; k++)
            {
                Vector2 velocity = spinningPoint.RotatedBy(radians * k);
                Dust dust = Dust.NewDustPerfect(player.Center, DustID.CrimsonTorch, velocity * 6f, 0, default, 2.5f);
                dust.noGravity = true;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                Dust dust2 = Dust.NewDustPerfect(player.Center, DustID.OrangeTorch, velocity * 10f, 0, default, 2.2f);
                dust2.noGravity = true;
                dust2.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                dust2.color = Color.Salmon;
                Dust dust3 = Dust.NewDustPerfect(player.Center, DustID.IchorTorch, velocity * 14f, 0, default, 1.9f);
                dust3.noGravity = true;
                dust3.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                dust3.color = Color.SandyBrown;
            }
            int holyExplosionDamage = (int)player.GetBestClassDamage().ApplyTo(LegacyAsgardsValorDashDamage);
            ModItem item = ItemLoader.GetItem(ItemType<AsgardsValorold>());
            Projectile.NewProjectile(player.GetSource_ItemUse(item.Item), player.Center, Vector2.Zero, ProjectileType<HolyExplosionold>(), holyExplosionDamage, 15f, Main.myPlayer, 0f, 0f);
            target.AddBuff(BuffType<HolyFlames>(), 180);
        }
    }
}
