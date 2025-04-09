using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using System;
using System.Collections.Generic;
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

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Celestusold : RogueWeapon, ILocalizedModType
    {
        public string SetRoute => $"{Generic.WeaponLocal}";
        public new string LocalizationCategory => $"{SetRoute}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 1222;
            Item.knockBack = 6f;
            Item.useAnimation = Item.useTime = 19;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.autoReuse = true;
            Item.shootSpeed = 27f;
            Item.shoot = ModContent.ProjectileType<CelestusBoomerang>();

            Item.width = 106;
            Item.height = 94;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool canStealth = player.Calamity().StealthStrikeAvailable();
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if(usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CelestusBoomerangExoLore>(), damage, knockback, player.whoAmI, 0f, 0f);
                if (canStealth)
                {
                    int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CelestusBoomerangExoLoreSteal>(), damage, knockback, player.whoAmI);
                    if (stealth.WithinBounds(Main.maxProjectiles))
                        Main.projectile[stealth].Calamity().stealthStrike = true;
                }
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
                if (canStealth)
                {
                    int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                    if (stealth.WithinBounds(Main.maxProjectiles))
                        Main.projectile[stealth].Calamity().stealthStrike = true;
                }
            }

            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                int locketDamage = player.ApplyArmorAccDamageBonusesTo((int)(damage * 0.8f));
                //改了下逻辑，让他在鼠标上方生成而非从……玩家头顶身上生成
                float srcPosX = Main.MouseWorld.X + Main.rand.NextFloat(-200f, 201f);
                float srcPosY = Main.MouseWorld.Y + Main.rand.NextFloat(-600f, -801f);
                //补一个特判，看鼠标位置是否低于玩家位置，如果低于则修改为玩家位置
                if (Main.MouseWorld.Y > player.Center.Y)
                    srcPosY = player.Center.Y + Main.rand.NextFloat(-600f, -801f);
                float pSpeed = Item.shootSpeed;
                //起点向量
                Vector2 srcPos = new (srcPosX, srcPosY);
                //距离向量
                Vector2 distVec = Main.MouseWorld - srcPos;
                //转为速度向量
                float dist = distVec.Length();
                dist = pSpeed / dist;
                distVec.X *= dist;
                distVec.Y *= dist;
                int p = Projectile.NewProjectile(source, srcPos, distVec, type, locketDamage, knockback * 0.5f, player.whoAmI);
                //允许其吃潜伏
                if (canStealth)
                {
                    Main.projectile[p].Calamity().stealthStrike = true;
                    Main.projectile[p].damage *= (int)1.5f;
                }
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Rogue.Celestusold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Rogue/CelestusoldGlow").Value);
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
