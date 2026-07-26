using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Shortsword;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class GalileoGladiusLegacy : CIMelee
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.damage = 92;
            Item.useAnimation = Item.useTime = 12;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileType<GalileoGladiusProjLegacy>();
            Item.shootSpeed = 3f;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override bool MeleePrefix() => true;

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.Gladius).
                    AddIngredient(CalamityMaterials.Lumenyl, 10).
                    AddIngredient(CalamityMaterials.RuinousSoul, 5).
                    AddIngredient(CalamityMaterials.ExodiumCluster, 15).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.Gladius).
                    AddIngredient<GalacticaSingularity>(5).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
