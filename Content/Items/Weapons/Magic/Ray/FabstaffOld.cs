using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class FabstaffOld : CIMagic, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 3456;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 42;
            Item.width = 84;
            Item.height = 84;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FabRayOld>();
            Item.shootSpeed = 6f;

            Item.Calamity().devItem = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Necroplasm>(100)
                .AddIngredient<ShadowspecBar>(50)
                .AddTile(ModContent.TileType<DraedonsForge>())
                .Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
