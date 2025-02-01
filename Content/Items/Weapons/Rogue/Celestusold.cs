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
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.ExoLore;
using Terraria.Localization;
using CalamityInheritance.Content.Projectiles.Ranged;

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

        public override float StealthDamageMultiplier => 0.6f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            if(usPlayer.exoMechLore)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CelestusBoomerangExoLore>(), damage, knockback, player.whoAmI, 0f, 0f);
                if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
                {
                    int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CelestusBoomerangExoLoreSteal>(), damage, knockback, player.whoAmI);
                    if (stealth.WithinBounds(Main.maxProjectiles))
                        Main.projectile[stealth].Calamity().stealthStrike = true;
                }
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
                {
                    int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                    if (stealth.WithinBounds(Main.maxProjectiles))
                        Main.projectile[stealth].Calamity().stealthStrike = true;
                }
            }

            if (usPlayer.exoMechLore)
            {
                float veneratedCloneSpeed = Item.shootSpeed;
                Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float veneratedCloneXPos = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                float veneratedCloneYPos = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (player.gravDir == -1f)
                {
                    veneratedCloneYPos = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - realPlayerPos.Y;
                }
                float veneratedCloneDistance = (float)Math.Sqrt((double)(veneratedCloneXPos * veneratedCloneXPos + veneratedCloneYPos * veneratedCloneYPos));
                if ((float.IsNaN(veneratedCloneXPos) && float.IsNaN(veneratedCloneYPos)) || (veneratedCloneXPos == 0f && veneratedCloneYPos == 0f))
                {
                    veneratedCloneXPos = (float)player.direction;
                    veneratedCloneYPos = 0f;
                    veneratedCloneDistance = veneratedCloneSpeed;
                }
                else
                {
                    veneratedCloneDistance = veneratedCloneSpeed / veneratedCloneDistance;
                }

                realPlayerPos = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                realPlayerPos.Y -= 100f;
                veneratedCloneXPos = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                veneratedCloneYPos = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (veneratedCloneYPos < 0f)
                {
                    veneratedCloneYPos *= -1f;
                }
                if (veneratedCloneYPos < 20f)
                {
                    veneratedCloneYPos = 20f;
                }
                veneratedCloneDistance = (float)Math.Sqrt((double)(veneratedCloneXPos * veneratedCloneXPos + veneratedCloneYPos * veneratedCloneYPos));
                veneratedCloneDistance = veneratedCloneSpeed / veneratedCloneDistance;
                veneratedCloneXPos *= veneratedCloneDistance;
                veneratedCloneYPos *= veneratedCloneDistance;
                float speedX4 = veneratedCloneXPos + (float)Main.rand.Next(-30, 31) * 0.02f;
                float speedY5 = veneratedCloneYPos + (float)Main.rand.Next(-30, 31) * 0.02f;

                // 08DEC2023: Ozzatron: Locket + Old Fashioned may need to be a corner case. We should probably just rework Locket instead.
                int locketDamage = player.ApplyArmorAccDamageBonusesTo((int)(damage * 0.5f));

                int p = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, type, locketDamage, knockback * 0.5f, player.whoAmI);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();

            if (usPlayer.exoMechLore == true)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Rogue.Celestusold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
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

            CreateRecipe().
                AddIngredient<ElementalDisk>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
