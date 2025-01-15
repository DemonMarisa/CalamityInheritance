using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class BloodyRupture : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 15;
            Item.width = 28;
            Item.height = 28;
            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<BloodyRuptureProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe.AddIngredient(ItemID.Vertebrae, 2);
            recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
