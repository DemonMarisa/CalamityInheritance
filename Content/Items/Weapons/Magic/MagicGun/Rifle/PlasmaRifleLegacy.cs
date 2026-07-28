using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicGun.Rifle;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Rifle
{
    public class PlasmaRifleLegacy : CIMagic
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 20;
            Item.damage = 150;
            Item.mana = 40;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = CISounds.PlasmaBlast;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<PlasmaShot>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool AltFunctionUse(Player player) => true;

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
                mult *= 0.25f;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 1f;
            return 0.2f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, ProjectileType<PlasmaBolt>(), damage * 2, knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ToxicFlask).
                AddIngredient(ItemID.LaserRifle).
                AddIngredient(CalamityMaterials.UelibloomBar, 7).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
