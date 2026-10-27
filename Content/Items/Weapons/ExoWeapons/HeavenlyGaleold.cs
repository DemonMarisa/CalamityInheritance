using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Ranged.Bow;
using CalamityInheritance.Content.Projectiles.ExoWeapons.ExoArrow;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class HeavenlyGaleold : CIExoWeapon
    {

        public const float NormalArrowDamageMult = 1.25f;
        private static int[] ExoArrows;
        private static int[] ExoArrowsExoLore;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ExoArrows =
            [
                ProjectileType<ExoArrowTeal>(),
                ProjectileType<OrangeExoArrow>(),
                ProjectileType<ExoArrowGreen>(),
                ProjectileType<ExoArrowBlue>()
            ];
            ExoArrowsExoLore =
            [
                ProjectileType<ExoArrowTealExoLore>(),
                ProjectileType<ExoArrowOrangeExoLore>(),
                ProjectileType<ExoArrowOrangeExoLore>(),
                ProjectileType<ExoArrowGreenExoLore>(),
                ProjectileType<ExoArrowBlueExoLore>()
            ];
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 708;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 98;
            Item.useTime = 11;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 baseOffset = velocity;
            baseOffset.Normalize();
            baseOffset *= 40f;

            float piOver10 = MathHelper.Pi / 10f;
            bool againstWall = !Collision.CanHit(source, 0, 0, source + baseOffset, 0, 0);

            int numArrows = UseAlt ? 7 : 5;

            for (int i = 0; i < numArrows; i++)
            {
                float offsetAmt = i - (numArrows - 1f) / 2f;
                Vector2 offset = baseOffset.RotatedBy(piOver10 * offsetAmt);

                if (againstWall)
                    offset -= baseOffset;

                int thisArrowType;
                if (UseAlt)
                    thisArrowType = Main.rand.Next(ExoArrowsExoLore);
                else
                    thisArrowType = Main.rand.Next(ExoArrows);

                int proj = Projectile.NewProjectile(spawnSource, source + offset, velocity, thisArrowType, damage, knockback, player.whoAmI);
                if (type != ProjectileID.WoodenArrowFriendly)
                    Main.projectile[proj].noDropItem = true;
            }

            return false;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 66)
                return false;
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TelluricGlareLegacy>().
                AddIngredient<ClockBowLegacy>().
                AddIngredient<Phangasm>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<TelluricGlareLegacy>().
                AddIngredient<ClockBowLegacy>().
                AddIngredient<Phangasm>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
