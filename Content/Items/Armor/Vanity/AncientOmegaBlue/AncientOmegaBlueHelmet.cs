using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using Terraria.ID;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Armor.Vanity.AncientOmegaBlue
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientOmegaBlueHelmet: CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.vanity = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<AncientOmegaBlueLeggings>() && legs.type == ItemType<AncientOmegaBlueChestplate>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<Necroplasm>(2).
            AddIngredient<ReaperTooth>(1).
            AddTile(TileID.LunarCraftingStation).
            Register();
        }
    }
}
