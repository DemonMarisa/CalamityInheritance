using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;
using CalamityInheritance.Core;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    // TODO -- This weapon is a disgrace to its Armored Core heritage. It needs a full rework.
    public class ACTKarasawa : CIRanged, ILocalizedModType
    {
        public static readonly SoundStyle FireSound = new("CalamityMod/Sounds/Item/MechGaussRifle");

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 44;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 14687;
            Item.knockBack = 12f;
            Item.useTime = 52;
            Item.useAnimation = 52;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = FireSound;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<AlgtPink>() : ModContent.RarityType<DarkBlue>();
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<ACTKarasawaHoldout>();
            Item.shootSpeed = 1f;
            Item.useAmmo = AmmoID.None;
            Item.channel = true;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 46;
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ACTKarasawaHoldout>()] < 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 targetPosition = Main.MouseWorld;
            player.itemRotation = CIFunction.CalculateItemRotation(player, targetPosition, -18);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LargeRuby).
                AddIngredient<MysteriousCircuitry>(15).
                AddIngredient<DubiousPlating>(25).
                AddIngredient<GalacticaSingularity>(5).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<NightmareFuel>(20).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
