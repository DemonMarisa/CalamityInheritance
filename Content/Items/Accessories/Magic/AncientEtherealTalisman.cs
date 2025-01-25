using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Magic
{
    public class AncientEtherealTalisman : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Magic";
        public override void SetStaticDefaults()
        {
            // //微光嬗变config开启时允许这个原灾的空灵护符与该物品相互转化
            // if(CalamityInheritanceConfig.Instance.CustomShimmer == true)
            // {
            //     ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<EtherealTalisman>()] = ModContent.ItemType<AncientEtherealTalisman>();
            //     ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AncientEtherealTalisman>()] = ModContent.ItemType<EtherealTalisman>();
            // }
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eTalisman = true;

            player.manaMagnet = true;
            if (!hideVisual)
                player.manaFlower = true;

            player.statManaMax2 += 150;
            player.GetDamage<MagicDamageClass>() += 0.20f;
            player.manaCost *= 0.8f;
            player.GetCritChance<MagicDamageClass>() += 15;
            player.pStone = true;
            player.lifeRegen += 1;
        }

        public override void AddRecipes()
        {
            // //关闭则启用普通的合成表
            // if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
            // {
                CreateRecipe().
                    AddIngredient<SigilofCalamitas>().
                    AddRecipeGroup("AnyManaFlower").
                    AddIngredient(ItemID.CharmofMyths).
                    AddIngredient(ItemID.LunarBar, 8).
                    AddIngredient<GalacticaSingularity>(4).
                    AddIngredient<AscendantSpiritEssence>(4).
                    AddTile<CosmicAnvil>().
                    Register();
            // }
        }
    }
}
