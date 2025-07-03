using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee.Spear;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class VictideSpear :CIMelee, ILocalizedModType 
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.height = 46;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<VictideSpearProj>();
            Item.shootSpeed = 4f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                int thrown = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<VictideSpearProj>(), damage, knockback, player.whoAmI);
                Main.projectile[thrown].CalamityInheritance().ThrownMode = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                int notThrown = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<VictideSpearProj>(), damage, knockback, player.whoAmI);
                Main.projectile[notThrown].CalamityInheritance().ThrownMode = false;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
