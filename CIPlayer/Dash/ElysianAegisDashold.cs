using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Accessories;
using LAP.Core.GlobalInstance.Players.DashSystem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class ElysianAegisDashold : BasePlayerDash
    {
        public static int LegacyElysianAegisDashDamage = 1000;
        public static float LegacyElysianAegisDashKnockback = 13f;
        public override int ImmuneTime(Player player) => 15;
        public override int DashTime(Player player) => 15;
        public override int DashDelay(Player player) => 24;
        public override DashDamageInfo DashDamageInfo(Player player) => new DashDamageInfo(LegacyElysianAegisDashDamage, LegacyElysianAegisDashKnockback, DamageClass.Melee);
        public override float DashSpeed(Player player) => 22f;
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
            for (int d = 0; d < 4; d++)
            {
                Dust hFlameDust = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (player.velocity * 1.2f), Main.rand.NextBool(8) ? 222 : 162, -player.velocity.RotatedByRandom(MathHelper.ToRadians(10f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(1.8f, 2.8f));
                hFlameDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                hFlameDust.noGravity = hFlameDust.type == 222 ? false : true;
                hFlameDust.fadeIn = 0.5f;
                if (hFlameDust.type == 222)
                {
                    hFlameDust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                    hFlameDust.velocity += new Vector2(0, -2.5f) * Main.rand.NextFloat(0.8f, 1.2f);
                }
                if (hFlameDust.type == 180)
                    hFlameDust.scale = Main.rand.NextFloat(1.6f, 2.2f);
                Dust dust = Dust.NewDustPerfect(player.Center + Main.rand.NextVector2Circular(6, 6) - player.velocity * 2, 228);
                dust.velocity = -player.velocity * Main.rand.NextFloat(0.6f, 1.4f);
                dust.scale = Main.rand.NextFloat(0.9f, 1.4f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int DamageDone)
        {
            int supremeExplosionDamage = (int)player.GetBestClassDamage().ApplyTo(ElysianAegis.RamExplosionDamage);
            ModItem item = ItemLoader.GetItem(ItemType<ElysianAegisold>());
            Projectile.NewProjectile(player.GetSource_ItemUse(item.Item), player.Center, Vector2.Zero, ProjectileType<HolyExplosionold>(), supremeExplosionDamage, ElysianAegis.RamExplosionKnockback, Main.myPlayer, 3f, 0f);
            target.AddBuff(BuffType<HolyFlames>(), 300);
        }
    }
}
