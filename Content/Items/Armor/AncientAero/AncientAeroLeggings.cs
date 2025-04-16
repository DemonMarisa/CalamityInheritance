using CalamityMod.Items.Materials;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAero
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAeroLeggings : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.defense = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += 0.5f;
            base.UpdateEquip(player);
        }
        public override void AddRecipes()
        {
        }
    }
}