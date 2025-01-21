using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Rogue;
using Microsoft.Xna.Framework.Graphics;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Celestusold : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetDefaults()
        {
            Item.damage = 280;
            Item.knockBack = 6f;
            Item.useAnimation = Item.useTime = 20;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.autoReuse = true;
            Item.shootSpeed = 25f;
            Item.shoot = ModContent.ProjectileType<CelestusBoomerang>();

            Item.width = 106;
            Item.height = 94;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override float StealthDamageMultiplier => 0.8f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/CelestusoldGlow").Value);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalDisk>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
