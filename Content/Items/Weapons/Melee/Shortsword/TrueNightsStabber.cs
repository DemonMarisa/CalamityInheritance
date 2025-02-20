using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class TrueNightsStabber : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 16;
            Item.width = 40;
            Item.height = 40;
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TrueNightsStabberProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<NightsStabber>());
            recipe.AddIngredient(ItemID.SoulofSight, 30);
            recipe.AddIngredient(ItemID.SoulofMight, 30);
            recipe.AddIngredient(ItemID.SoulofFright, 30);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
