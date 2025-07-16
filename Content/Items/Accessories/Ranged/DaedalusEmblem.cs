using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class DaedalusEmblem : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 40;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.defense = 10;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (!modPlayer.deadshotBrooch)
                player.CIMod().DeadshotBroochCI = true;
            player.Calamity().rangedAmmoCost *= 0.8f;
            player.GetDamage<RangedDamageClass>() += 0.15f;
            player.GetCritChance<RangedDamageClass>() += 5;
            player.pickSpeed -= 0.15f;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CelestialStone).
                AddIngredient<CoreofCalamity>(2).
                AddIngredient(ItemID.RangerEmblem).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
