using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class SubsumingVortexold : CIMagic, ILocalizedModType
    {
        public static readonly SoundStyle[] TossSound =
        [
            CISoundMenu.VortexToss1,
            CISoundMenu.VortexToss2,
            CISoundMenu.VortexToss3
        ];
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 235;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 78;
            Item.width = 38;
            Item.height = 48;
            Item.UseSound = SoundID.Item84;
            Item.useTime = Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnormousConsumingVortexold>();
            Item.shootSpeed = 7f;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
            {
                Item.UseSound = Utils.SelectRandom(Main.rand, TossSound);
            }
            else Item.UseSound = SoundID.Item84;
            return true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 只比普通模式搞高一点
            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
                damage.Base *= 0.6f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, (Texture2D)ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Magic/SubsumingVortexoldGlow"));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if(usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(this.Item, 0, null), position, velocity * 4, ModContent.ProjectileType<EnormousConsumingVortexoldExoLore>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            }
            else
            {
                Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(this.Item, 0, null), position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f, 0f);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (player.CheckExoLore())
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Magic.SubsumingVortexold.ExoLoreOn");
                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AuguroftheVoid>()
                .AddIngredient<EventHorizon>()
                .AddIngredient<TearsofHeaven>()
                .AddIngredient<AuricBarold>(10)
                .DisableDecraft()
                .AddTile<DraedonsForge>().
                Register();
                
            CreateRecipe()
                .AddIngredient<AuguroftheVoid>()
                .AddIngredient<EventHorizon>()
                .AddIngredient<TearsofHeaven>()
                .AddIngredient<MiracleMatter>()
                .AddDecraftCondition(CalamityConditions.DownedExoMechs)
                .AddTile<DraedonsForge>().
                Register();

            CreateRecipe()
                .AddIngredient<AuguroftheVoid>()
                .AddIngredient<EventHorizon>()
                .AddIngredient<TearsofHeaven>()
                .AddIngredient<AncientMiracleMatter>()
                .AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter)
                .DisableDecraft()
                .AddTile<DraedonsForge>().
                Register();
        }
    }
}
