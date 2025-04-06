using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ShroomiteVisage : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 11; //62
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.ShroomiteBreastplate && legs.type == ItemID.ShroomiteLeggings;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.shroomiteStealth = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            base.UpdateArmorSet(player);
        }
        public override void UpdateEquip(Player player)
        {
            player.CIMod().ShroomiteFlameBooster = true;
            base.UpdateEquip(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ShroomiteBar, 12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
