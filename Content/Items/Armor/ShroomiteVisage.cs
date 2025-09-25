using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ShroomiteVisage : CIArmor, ILocalizedModType
    {
        
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
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemID.ShroomiteBreastplate && legs.type == ItemID.ShroomiteLeggings;
        public override void UpdateArmorSet(Player player)
        {
            player.shroomiteStealth = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            base.UpdateArmorSet(player);
        }
        public override void UpdateEquip(Player player)
        {
            if (player.HeldItem.useAmmo == AmmoID.Gel)
            {
                player.GetDamage<RangedDamageClass>() += 0.30f;
                player.GetCritChance<RangedDamageClass>() += 5;
                if (Main.zenithWorld)
                    player.GetDamage<RangedDamageClass>() *= 15f;
            }
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
