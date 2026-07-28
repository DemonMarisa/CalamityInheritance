using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicGun.HandGun;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.HandGun
{
    public class LazharLegacy : CIMagic
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 20;
            Item.damage = 76;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileType<LazharSolarBeam>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpaceGun).
                AddIngredient(ItemID.HeatRay).
                AddIngredient(ItemID.FragmentSolar, 6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
