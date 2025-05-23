﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("RogueTypeGalaxySmasher")]
    public class RogueTypeHammerGalaxySmasher : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 325;
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerGalaxySmasherProj>();
            Item.shootSpeed = 20f;
        }
        public override float StealthDamageMultiplier => 1.20f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool stealth = player.CheckStealth();
            if (!stealth)
                return true;

            int t = Projectile.NewProjectile(source, position, velocity ,type, (int)(damage*1.15f), knockback, player.whoAmI, 0f, 0f, -3f);
            Main.projectile[t].Calamity().stealthStrike = true;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueTypeHammerStellarContempt>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
