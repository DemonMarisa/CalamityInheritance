using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public partial class CalamityInheritanceGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int timesUsed = 0;
        #region GrabChanges
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            int itemGrabRangeBoost = 0 +
                (modPlayer.wallOfFleshLore ? 100 : 0) +
                (modPlayer.planteraLore ? 150 : 0) +
                (modPlayer.polterghastLore ? 300 : 0);

            grabRange += itemGrabRangeBoost;
        }
        #endregion
        #region Shoot
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();

            if (modPlayer.wallOfFleshLore)
                velocity *= 1.10f;
            if (modPlayer.planteraLore)
                velocity *= 1.15f;
            if (modPlayer.polterghastLore)
                velocity *= 1.20f;
        }
        #endregion

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            CalamityPlayer modPlayer1 = player.Calamity();
            if (modPlayer.godSlayerRangedold && modPlayer1.canFireGodSlayerRangedProjectile)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    modPlayer1.canFireGodSlayerRangedProjectile = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        // God Slayer Ranged Shrapnel: 100%, soft cap starts at 800 base damage
                        int shrapnelRoundDamage = CalamityUtils.DamageSoftCap(damage * 2, 1500);
                        shrapnelRoundDamage = player.ApplyArmorAccDamageBonusesTo(shrapnelRoundDamage);

                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<GodSlayerShrapnelRound>(), shrapnelRoundDamage, 2f, player.whoAmI);
                    }
                }
            }

            if (modPlayer.AuricbloodflareRangedSoul && modPlayer1.canFireBloodflareRangedProjectile)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    modPlayer1.canFireBloodflareRangedProjectile = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        // Bloodflare Ranged Bloodsplosion: 80%, soft cap starts at 150 base damage
                        // This is intentionally extremely low because this effect can be grossly overpowered with sniper rifles and the like.
                        int bloodsplosionDamage = CalamityUtils.DamageSoftCap(damage * 0.8, 1200);
                        bloodsplosionDamage = player.ApplyArmorAccDamageBonusesTo(bloodsplosionDamage);

                        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BloodBomb>(), bloodsplosionDamage, 2f, player.whoAmI);
                    }
                }
            }

            if (modPlayer.reaverRangedRocket && modPlayer.canFireReaverRangedRocket)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    modPlayer.canFireReaverRangedRocket = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity* 0.001f, ModContent.ProjectileType<reaverRangedRocketMark>(), damage, 2f, player.whoAmI, 0f, 0f);
                    }
                }
            }
            return true;
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (CalamityInheritanceConfig.Instance.turnoffCorner == false)
            {
                if (item.ModItem != null && item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
                {
                    Texture2D iconTexture = ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/Mark").Value;
                    Vector2 iconPosition = position + new Vector2(4f, 4f);
                    float iconScale = 0.45f;

                    spriteBatch.Draw(iconTexture, iconPosition, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
