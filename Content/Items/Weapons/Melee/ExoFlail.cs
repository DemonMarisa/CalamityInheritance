using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class ExoFlail : CIMelee, ILocalizedModType
    {
        

        public static float Speed = 34f;

        public static float MouseHomingAcceleration = 0.75f;

        public static float MaxRange = 780f;

        public static float ReturnSpeed = 40f;

        private int hitCount = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 90;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.damage = 1000;
            Item.knockBack = 9f;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item101;
            Item.channel = true;
            Item.rare = CIConfig.Instance.SpecialRarityColor?ModContent.RarityType<SeraphPurple>():ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ModContent.ProjectileType<ExoFlailProj>();
            Item.shootSpeed = 24f;
        }
        public override bool MeleePrefix() => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if(usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                hitCount++;
                float ai3 = (Main.rand.NextFloat() - 0.75f) * 0.7853982f;
                if (hitCount >= 5)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ExoFlailProj2>(), damage, knockback, player.whoAmI, 0f);
                    hitCount = 0;
                }
                else
                {
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, ai3);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ExoFlailProj2>(), damage, knockback, player.whoAmI, 0f);
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/ExoFlailGlow").Value);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Melee.ExoFlail.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DragonPow>().
                AddIngredient<PulseDragon>().
                AddIngredient<CrescentMoon>().
                AddIngredient<ClamCrusher>().
                AddIngredient<AuricBarold>(10).
                DisableDecraft().
                AddTile<DraedonsForgeold>().
                Register();

            CreateRecipe().
                AddIngredient<DragonPow>().
                AddIngredient<PulseDragon>().
                AddIngredient<CrescentMoon>().
                AddIngredient<ClamCrusher>().
                AddIngredient<MiracleMatter>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddTile<DraedonsForge>().
                Register();

             CreateRecipe().
                AddIngredient<DragonPow>().
                AddIngredient<PulseDragon>().
                AddIngredient<CrescentMoon>().
                AddIngredient<ClamCrusher>().
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
