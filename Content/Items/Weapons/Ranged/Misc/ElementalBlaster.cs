using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Ranged.SMG;
using CalamityInheritance.Content.Projectiles.Ranged.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Misc
{
    public class ElementalBlaster : CIRanged, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 104;
            Item.height = 42;
            Item.damage = 67;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 2;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.75f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = CISounds.PlasmaBolt;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<RainbowBlast>();
            Item.shootSpeed = 18f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -12);
        }
        public override void UseItemFrame(Player player)
        {
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 offset = new Vector2(20, 0);
            Projectile.NewProjectile(source, position + offset.RotatedBy(velocity.ToRotation()) + new Vector2(0, -12), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PaintballBlaster>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
