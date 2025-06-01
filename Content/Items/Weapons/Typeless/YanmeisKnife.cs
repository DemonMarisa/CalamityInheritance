using CalamityMod.Items.Materials;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Typeless;

namespace CalamityInheritance.Content.Items.Weapons.Typeless
{
    public class YanmeisKnife : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Typeless";

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 44;
            Item.damage = 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4.5f;
            Item.autoReuse = false;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.Calamity().donorItem = true;
            Item.UseSound = SoundID.Item71;
            Item.shoot = ModContent.ProjectileType<YanmeisKnifeSlash>();
            Item.shootSpeed = 24f;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.ClasslessWeapon;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 6;

        public override bool CanUseItem(Player player)
        {
            if (player.Calamity().KameiBladeUseDelay > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.Calamity().KameiBladeUseDelay = 180;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PsychoKnife).
                AddIngredient(ItemID.Obsidian, 10).
                AddRecipeGroup("IronBar", 20).
                AddIngredient<PlagueCellCanister>(50).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
