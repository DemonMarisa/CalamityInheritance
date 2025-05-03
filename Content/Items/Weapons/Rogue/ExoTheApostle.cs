using CalamityMod.Items.Weapons.Rogue;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.Audio;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.ExoLore;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class ExoTheApostle : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public static readonly SoundStyle ThrowSound1 = new("CalamityMod/Sounds/Item/RealityRupture") { Volume = 1.2f, PitchVariance = 0.3f };
        public static readonly SoundStyle ThrowSound2 = new("CalamityInheritance/Sounds/Custom/ExoApostleStealth") { Volume = 1.2f, PitchVariance = 0.3f };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 5555;
            Item.width = 92;
            Item.height = 100;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = CIConfig.Instance.SpecialRarityColor?ModContent.RarityType<SeraphPurple>():ModContent.RarityType<CatalystViolet>();
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ExoSpearProj>();
            Item.shootSpeed = 16f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            bool onExo = player.CheckExoLore();
            bool isStealth = player.CheckStealth();
            //无作用
            int getBuffDamage = damage;
            //无Lore时影响的射弹类型
            int pType = isStealth ? ModContent.ProjectileType<ExoSpearProjNorSteal>() : ModContent.ProjectileType<ExoSpearProjNor>();
            //开启lore时被影响的射弹类型
            int pTypeLore = isStealth ? ModContent.ProjectileType<ExoSpearStealthProj>() : type;
            //往后的投枪速度
            Vector2 backSpeed = isStealth ? -velocity * 3.5f : -velocity * 1.5f;
            //PlayeSound
            SoundStyle TossSound = isStealth ? ThrowSound2 : ThrowSound1;
            if (onExo)
            {
                
                Projectile.NewProjectileDirect(source, position, backSpeed, ModContent.ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
                int p = Projectile.NewProjectile(source, position, velocity * 1.25f, pTypeLore, damage, knockback, player.whoAmI);
                if (p.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[p].Calamity().stealthStrike = isStealth;
                    Main.projectile[p].CalamityInheritance().PingReducedNanoFlare = true;
                    Main.projectile[p].usesLocalNPCImmunity = isStealth;
                }
                SoundEngine.PlaySound(TossSound, player.Center);
            }
            else
            {
                int p = Projectile.NewProjectile(source, position, velocity * 1.5f, pType, damage, knockback, player.whoAmI);
                Main.projectile[p].Calamity().stealthStrike = isStealth;
                Main.projectile[p].CalamityInheritance().PingReducedNanoFlare = true;
                //潜伏多扔一把。
                if (isStealth)
                {
                    int j = Projectile.NewProjectile(source, position, velocity * 1.5f, pType, damage, knockback, player.whoAmI);
                    Main.projectile[j].Calamity().stealthStrike = isStealth;
                    Main.projectile[j].CalamityInheritance().PingReducedNanoFlare = true;
                }
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Rogue/ExoTheApostleGlow").Value);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Rogue.ExoTheApostle.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DragonSpear>().
                AddIngredient<StormfrontRazor>().
                AddIngredient<ShardofAntumbra>(500).
                AddIngredient<PhantasmalRuinold>().
                AddIngredient<EclipseSpear>().
                AddIngredient<TarragonThrowingDart>(500).
                DisableDecraft().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeold>().
                Register();

            CreateRecipe().
                AddIngredient<Wrathwing>().
                AddIngredient<StormfrontRazor>().
                AddIngredient<ShardofAntumbra>(500).
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalRuin").
                AddIngredient<EclipsesFall>().
                AddIngredient<TarragonThrowingDart>(500).
                AddIngredient<MiracleMatter>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddTile<DraedonsForge>().
                Register();
            
            CreateRecipe().
                AddIngredient<Wrathwing>().
                AddIngredient<StormfrontRazor>().
                AddIngredient<ShardofAntumbra>(500).
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalRuin").
                AddIngredient<EclipsesFall>().
                AddIngredient<TarragonThrowingDart>(500).
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
