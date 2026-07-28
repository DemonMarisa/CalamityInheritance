using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bow
{
    public class AstralBow : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 78;
            Item.damage = 92;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 6;
            Item.useAnimation = 18;
            Item.reuseDelay = 18;
            Item.useLimitPerAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 25;
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.AstralBar, 7).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<GalacticaSingularity>(4).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
