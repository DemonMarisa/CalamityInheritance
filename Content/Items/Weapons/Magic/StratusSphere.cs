using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.Ores;
using CalamityInheritance.Content.Projectiles.Magic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Projectiles.Magic;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class StratusSphere : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 251;
            Item.DamageType = DamageClass.Magic;
            Item.width = 22;
            Item.height = 24;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.shoot = ModContent.ProjectileType<StratusSphereHold>();
            Item.shootSpeed = 3.5f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.mana = 30;
            Item.knockBack = 2;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = ModContent.RarityType<PureGreen>();
        }
        public override void OnConsumeMana(Player player, int manaConsumed) => player.statMana += manaConsumed;

        // This weapon uses a holdout projectile.
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<StratusSphereHold>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 5);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster> (), 12);
            recipe.AddIngredient(ItemID.NebulaArcanum);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
