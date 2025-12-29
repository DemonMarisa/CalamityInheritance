using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
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
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Celestusold : RogueWeapon, ILocalizedModType
    {
        public static string SetRoute => $"{Generic.BaseWeaponCategory}";
        public const float SetProjSpeed = 27f;
        public SoundStyle[] getSound =
            [
                CISoundMenu.CelestusToss1,
                CISoundMenu.CelestusToss2,
                CISoundMenu.CelestusToss3
            ];
        public new string LocalizationCategory => $"{SetRoute}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 600;
            Item.knockBack = 6f;
            Item.useAnimation = Item.useTime = 19;
            Item.DamageType = GetInstance<RogueDamageClass>();
            Item.autoReuse = true;
            Item.shootSpeed = SetProjSpeed;
            Item.shoot = ProjectileType<CelestusBoomerang>();

            Item.width = 106;
            Item.height = 94;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
        }
        public override bool CanUseItem(Player player)
        {
            if ((player.CIMod().LoreExo || player.CIMod().PanelsLoreExo) && player.Calamity().StealthStrikeAvailable())
            {
                Item.UseSound = Utils.SelectRandom(Main.rand, getSound);
            }
            else Item.UseSound = CISoundID.SoundWeaponSwing;
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool canStealth = player.Calamity().StealthStrikeAvailable();
            bool onLore = player.CheckExoLore();
            int pTypeLore = canStealth ? ProjectileType<CelestusBoomerangExoLoreSteal>() : ProjectileType<CelestusBoomerangExoLore>(); 
            if (onLore)
            {
                int t = Projectile.NewProjectile(source, position, velocity, pTypeLore, damage, knockback, player.whoAmI);
                Main.projectile[t].Calamity().stealthStrike = canStealth;
                // 全部修改为了从鼠标X与玩家X的中点处发射
                int fireOffset = -800;
                Vector2 mousePos = Main.MouseWorld;
                int firePosX = (int)(mousePos.X + player.Center.X) / 2;
                int firePosY = (int)player.Center.Y + fireOffset;
                Vector2 firePos = new(firePosX + Main.rand.Next(-100, 100), firePosY);

                Vector2 direction = mousePos - firePos;
                direction.Normalize();
                Vector2 newVelocity = direction * velocity.Length();

                int p = Projectile.NewProjectile(source, firePos, newVelocity, ProjectileType<CelestusBoomerangExoLore>(), (int)(damage * 0.8f), knockback * 0.5f, player.whoAmI);
                //允许其吃潜伏
                if (canStealth)
                {
                    Main.projectile[p].Calamity().stealthStrike = true;
                    Main.projectile[p].damage *= (int)1.5f;
                }
            }
            else
            {
                bool ifStealth = player.CheckStealth();
                if (ifStealth)
                {
                    int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                }
                else
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
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
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, Request<Texture2D>($"{Generic.WeaponPath}/Rogue/CelestusoldGlow").Value);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ReboundingRainbow>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                DisableDecraft().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeold>().
                Register();

            CreateRecipe().
                AddIngredient<ReboundingRainbow>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                AddIngredient<MiracleMatter>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<ReboundingRainbow>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
