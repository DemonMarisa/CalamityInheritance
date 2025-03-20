using CalamityMod;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer.Dash;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    public class StatisVoidSash : CIAccessories, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value= CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            player.GetDamage<GenericDamageClass>() += 0.10f;
            //Scarlet:移除栏位加成，伤害加成上调至10%，移除暴击概率加成（已经够多了)
            player.jumpSpeedBoost += 3.2f;
            player.moveSpeed += 0.10f;
            player.spikedBoots = 2;
            player.noFallDmg = true;
            player.blackBelt = true;
            player.autoJump = true;
            player.Calamity().DashID = string.Empty;
            player.dashType = 0;
            modPlayer1.CIDashID = StatisVoidSashDashOld.ID;   
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<StatisNinjaBelt>().
                AddIngredient<TwistingNether>(10).
                AddIngredient<NightmareFuel>(20).
                AddIngredient(ItemID.AvengerEmblem, 1).
                AddTile<CosmicAnvil>().
                Register();
                //Scarlet:合成材料增加复仇者徽章

            CreateRecipe().
                AddIngredient<StatisNinjaBeltLegacy>().
                AddIngredient<TwistingNether>(10).
                AddIngredient<NightmareFuel>(20).
                AddIngredient(ItemID.AvengerEmblem, 1).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
