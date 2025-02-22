using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Wulfrum;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.Wulfrum
{
    public class WulfrumStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 2;
            Item.width = 44;
            Item.height = 46;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WulfrumBoltOld>();
            Item.shootSpeed = 9f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, -1);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo projSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(projSource, position, velocity, type, damage, knockback, player.whoAmI);
            
            Vector2 targetPosition = Main.MouseWorld;
            player.itemRotation = CalamityInheritanceUtils.CalculateItemRotation(player, targetPosition, -18);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap> (12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
