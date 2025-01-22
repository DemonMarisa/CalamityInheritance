using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class YharimsGiftLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public int dragonTimer = 60;

        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<YharimsGift>()] = ModContent.ItemType<YharimsGiftLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<YharimsGiftLegacy>()] = ModContent.ItemType<YharimsGift>();
            }
        }
        public override void SetDefaults()
        {
            Item.defense = 30;
            Item.width = 20;
            Item.height = 22;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var source = player.GetSource_Accessory(Item);
            player.moveSpeed += 0.15f;
            player.GetDamage<GenericDamageClass>() += 0.15f;
            if (!player.StandingStill())
            {
                dragonTimer--;
                if (dragonTimer <= 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = (int)player.GetBestClassDamage().ApplyTo(175);
                        int projectile1 = Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<DragonDust>(), damage, 5f, player.whoAmI, 0f, 0f);
                        Main.projectile[projectile1].timeLeft = 60;
                    }
                    dragonTimer = 60;
                }
            }
            else
            {
                dragonTimer = 60;
            }
            if (player.immune)
            {
                if (player.miscCounter % 8 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = (int)player.GetBestClassDamage().ApplyTo(375);
                        CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<SkyFlareFriendly>(), damage, 9f, player.whoAmI);
                    }
                }
            }
        }
        public override void AddRecipes()
        {

            if(CalamityInheritanceConfig.Instance.CustomShimmer == false) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            {
                CreateRecipe().
                AddIngredient<YharimsGift>().
                AddIngredient(ItemID.IronskinPotion, 5).
                AddTile(TileID.AlchemyTable).
                Register();
            }
        }
    }
}
