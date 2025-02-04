using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TerraDiskMeleeLegacy: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static readonly float Speed = 12f;

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 64;
            Item.damage = 100;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 30;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.shoot = ModContent.ProjectileType<TerraDiskProjectileLegacyMelee>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Equanimity>().
                AddIngredient<FishboneBoomerang>().
                AddIngredient(ItemID.ThornChakram, 1).
                AddIngredient<LivingShard>(8).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
