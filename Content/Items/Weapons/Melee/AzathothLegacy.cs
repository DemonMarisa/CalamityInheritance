using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class AzathothLegacy : ModItem, ILocalizedModType
    {
        public int NewDamage = CIServerConfig.Instance.ShadowspecBuff ? 650 : 270;
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = NewDamage; //90->600, 90的面板破的了防?
            Item.knockBack = 6f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<AzathothYoyoLegacy>();
            Item.shootSpeed = 24f;

            Item.autoReuse = true;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.Calamity().devItem = true;
            Item.value = CIShopValue.RarityPriceDonatorPink;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Terrarian).
                AddIngredient<CoreofCalamity>(2).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
