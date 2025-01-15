using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class CarnageRay : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BloodRayold>();
            Item.shootSpeed = 6f;
        }
        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, -2);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CrimsonRod).
                AddIngredient(ItemID.MagicMissile).
                AddIngredient(ItemID.WandofSparking).
                AddIngredient(ItemID.AmberStaff).
                AddIngredient<PurifiedGel>(10).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
