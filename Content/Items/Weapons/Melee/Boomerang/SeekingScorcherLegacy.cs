using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.Melee.LightGreadtSword;
using CalamityInheritance.Content.Projectiles.Melee.Boomerang;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class SeekingScorcherLegacy : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 62;
            Item.damage = 232;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 17;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.shoot = ProjectileType<DivineHatchetBoomerang>();
            Item.shootSpeed = 14f;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
        }
        public override void AddRecipes()
        {
            CreateCalRecipe(Type).
                AddCalIngredient(ItemID.PossessedHatchet).
                AddCalIngredient(CalamityMaterials.DivineGeode, 5).
                AddCalIngredient(CalamityMaterials.UnholyEssence, 8).
                AddCalTile(TileID.LunarCraftingStation).
                RegisterCal();
        }
    }
}
