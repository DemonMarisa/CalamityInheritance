using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class SubsumingVortexold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetDefaults()
        {
            Item.damage = 121;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 78;
            Item.width = 38;
            Item.height = 48;
            Item.UseSound = SoundID.Item84;
            Item.useTime = Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Red;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnormousConsumingVortexold>();
            Item.shootSpeed = 7f;
        }
        public override bool CanUseItem(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            if (usPlayer.LoreExo)
            {
                Item.damage = 121;
            }
            else
            {
                Item.damage = 195;
            }
            return base.CanUseItem(player);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, (Texture2D)ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Magic/SubsumingVortexoldGlow"));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            if(usPlayer.LoreExo)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(this.Item, 0, null), position, velocity * 3, ModContent.ProjectileType<EnormousConsumingVortexoldExoLore>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
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
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();

            if (usPlayer.LoreExo == true)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Magic.SubsumingVortexold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AuguroftheElements>())
            .AddIngredient(ModContent.ItemType<EventHorizon>())
            .AddIngredient(ModContent.ItemType<TearsofHeaven>())
            .AddIngredient(ModContent.ItemType<MiracleMatter>())
            .AddTile(ModContent.TileType<DraedonsForge>()).
            Register();

            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AuguroftheElements>())
            .AddIngredient(ModContent.ItemType<EventHorizon>())
            .AddIngredient(ModContent.ItemType<TearsofHeaven>())
            .AddIngredient<AncientMiracleMatter>()
            .AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter)
            .AddTile(ModContent.TileType<DraedonsForge>()).
            Register();
        }
    }
}
