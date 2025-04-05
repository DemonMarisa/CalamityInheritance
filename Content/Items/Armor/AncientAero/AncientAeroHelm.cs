using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAero
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAeroHelm :CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.defense = 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AncientAeroArmor>() && legs.type == ModContent.ItemType<AncientAeroLeggings>();
        }
        public override void UpdateEquip(Player p)
        {
            p.moveSpeed += 0.1f;
            p.jumpSpeedBoost += 0.5f;
        }
        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            usPlayer.AncientAeroSet = true; 
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(15).
                AddIngredient(ItemID.FallenStar, 5).
                AddIngredient<HarpyRing>().
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}