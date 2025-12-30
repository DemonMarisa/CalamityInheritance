using CalamityInheritance.Content.Projectiles.HeldProj.Ranged.Cannos;
using CalamityInheritance.Rarity;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Cannos
{
    public class StarmadaLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Ranged;
        public override void SetDefaults()
        {
            Item.width = 122;
            Item.height = 50;
            Item.damage = 135;
            Item.knockBack = 15f;
            Item.shootSpeed = 16f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 27;
            Item.UseSound = SoundID.Item92;
            Item.shoot = ProjectileType<StarfleetMK2GunLegacy>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useTurn = false;
            Item.useAmmo = AmmoID.FallenStar;
            Item.autoReuse = true;
            Item.rare = RarityType<DeepBlue>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        // Spawning the holdout cannot consume ammo
        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool(3) && player.ownedProjectileCounts[Item.shoot] > 0;

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<StarfleetMK2GunLegacy>(), 0, 0f, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Starfleet>().
                AddIngredient<StarSputter>().
                AddIngredient<ExodiumCluster>(15).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<DarksunFragment>(8).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
