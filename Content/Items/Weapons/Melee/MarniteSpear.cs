using CalamityInheritance.Content.Projectiles.Melee.Spear;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class MarniteSpear: CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.damage = 26;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 21;
            Item.knockBack = 5.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 50;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ProjectileType<MarniteSpearProj>();
            Item.shootSpeed = 5f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.useStyle = ItemUseStyleID.Swing;
            else
                Item.useStyle = ItemUseStyleID.Shoot;
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //右键：投掷
            if (player.altFunctionUse == 2)
            {
                int thrown = Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.35f, velocity.Y * 1.35f, ProjectileType<MarniteThrowSpearProj>(), damage, knockback, player.whoAmI);
                Main.projectile[thrown].CalamityInheritance().ThrownMode = true;
            }
            //左键：正常矛
            else
            {
                int notThrown = Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.35f, velocity.Y * 1.35f, ProjectileType<MarniteSpearProj>(), damage, knockback, player.whoAmI);
                Main.projectile[notThrown].CalamityInheritance().ThrownMode = false;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyGoldBar", 5).
                AddIngredient(ItemID.GraniteBlock, 5).
                AddIngredient(ItemID.MarbleBlock, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}