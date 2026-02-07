using CalamityInheritance.Content.Projectiles.Rogue.Spears;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spears
{
    public class ShardofAntumbraLegacy : CIRogueClass
    {
        public override void ExSD()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 240;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<ShardofAntumbraLegacyProj>();
            Item.shootSpeed = 24f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeldConstruct>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
