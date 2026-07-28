using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Flamethrower;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Flamethrower
{
    public class BloodBoilerLegacy : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 145;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 30;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<BloodBoilerFireLegacy>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.BloodstoneCore, 6).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
