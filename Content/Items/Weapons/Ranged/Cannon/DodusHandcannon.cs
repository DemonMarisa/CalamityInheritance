using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.Ranged.HandGun;
using CalamityInheritance.Content.Projectiles.Ranged.Cannon;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Cannon
{
    public class DodusHandcannon : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 34;
            Item.damage = 1020;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;

            // Reduce volume to 30% so it stops destroying people's ears.
            Item.UseSound = CISounds.LargeWeaponFire with { Volume = 0.3f };

            Item.shoot = ProjectileType<HighExplosivePeanutShell>();
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<PlanteraGreen>();

        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = Item.shoot;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-17, 5);

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<PearlGodLegacy>().
                    AddIngredient(CalamityMaterials.RuinousSoul, 5).
                    AddIngredient(ItemID.LunarBar, 15).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<PearlGodLegacy>().
                    AddIngredient(ItemID.LunarBar, 15).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
