using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System.Text.RegularExpressions;
using CalamityMod.Projectiles.Rogue;
using Terraria.Audio;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class ExoTheApostle : RogueWeapon
    {
        public static readonly SoundStyle ThrowSound1 = new("CalamityMod/Sounds/Item/RealityRupture") { Volume = 1.2f, PitchVariance = 0.3f };
        public static readonly SoundStyle ThrowSound2 = new("CalamityInheritance/Sounds/Custom/ExoApostleStealth") { Volume = 1.2f, PitchVariance = 0.3f };
        public override void SetDefaults()
        {
            Item.damage = 9200;
            Item.width = 92;
            Item.height = 100;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ExoSpearProj>();
            Item.shootSpeed = 16f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.Calamity().StealthStrikeAvailable())
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, -velocity * 1.5f, ModContent.ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
            }
            SoundEngine.PlaySound(ThrowSound1, player.Center);
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                Projectile p = Projectile.NewProjectileDirect(source, position, -velocity * 3.5f, ModContent.ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
                int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ExoSpearStealthProj>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(ThrowSound2, player.Center);
                if (stealth.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                    Main.projectile[stealth].usesLocalNPCImmunity = true;
                }
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Wrathwing>());
            recipe.AddIngredient(ModContent.ItemType<ProfanedPartisan> ());
            recipe.AddIngredient(ModContent.ItemType<RealityRupture> ());
            recipe.AddIngredient(ModContent.ItemType<PhantasmalRuin>());
            recipe.AddIngredient(ModContent.ItemType<EclipsesFall>());
            recipe.AddIngredient(ModContent.ItemType<TarragonThrowingDart>(),500);
            recipe.AddIngredient(ModContent.ItemType<MiracleMatter>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
