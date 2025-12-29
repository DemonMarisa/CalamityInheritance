using CalamityInheritance.Content.Projectiles.Summon;
using CalamityMod.Items.Materials;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class SunSpiriteStaffLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Summon;
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 48;
            Item.damage = 12;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 35;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.15f;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<SunSpiritMinionLegacy>();
        }
        public override bool CanUseItem(Player player) => !player.HasProj<SunSpiritMinionLegacy>();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SandstoneBrick, 20).
                AddIngredient<StormlionMandible>(2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
