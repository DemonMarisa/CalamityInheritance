using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Verdant : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {

            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 86;
            Item.knockBack = 6f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<VerdantYoyo>();
            Item.shootSpeed = 16f;

            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UelibloomBar>(6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
