using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged.Bow;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bow
{
    public class Phangasm : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 82;
            Item.damage = 145;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PhangasmBowOld>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = RarityType<DeepBlue>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool(3) && player.ownedProjectileCounts[Item.shoot] > 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<PhangasmBowOld>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.Phantasm).
                    AddIngredient(CalamityMaterials.CosmiliteBar, 8).
                    AddIngredient(CalamityMaterials.NightmareFuel, 20).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.Phantasm).
                    AddTile<DraedonsForgeoldTile>().
                    Register();
            }
        }
    }
}
