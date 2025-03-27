using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Verdant : CIMelee, ILocalizedModType
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
            Item.damage = 98;
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
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.damage = 1800;
            }
            else
            {
                Item.damage = 98;
            }
            return default;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<UelibloomBar>(6).
                AddCondition(Condition.NotZenithWorld).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.Terrarian).
                AddIngredient<CoreofCalamity>(2).
                AddIngredient<ShadowspecBar>(5).
                AddCondition(Condition.ZenithWorld).
                AddDecraftCondition(Condition.ZenithWorld).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                AddCondition(Condition.ZenithWorld).
                DisableDecraft().
                Register();
        }
    }
}
