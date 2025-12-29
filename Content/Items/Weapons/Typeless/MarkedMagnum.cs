using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;
using CalamityInheritance.Content.Projectiles.Typeless;

namespace CalamityInheritance.Content.Items.Weapons.Typeless
{
    public class MarkedMagnum : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Typeless";

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.width = 54;
            Item.height = 20;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = false;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileType<MarkRound>();
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.ClasslessWeapon;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        // Marked Magnum scales off of all damage types simultaneously (meaning it scales 5x from universal damage boosts).
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellstoneBar, 7).
                AddIngredient(ItemID.Obsidian, 15).
                AddIngredient(ItemID.GlowingMushroom, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
