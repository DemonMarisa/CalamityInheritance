using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged.TrueScarlet;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Scarlet
{
    public class R99 : CIRanged, ILocalizedModType
    {
        public const int CrackedShieldTime = 300;
        public const int FleshHitTime = 310;
        public override void SetDefaults()
        {
            Item.width = 172;
            Item.height = 74;
            Item.damage = 14;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 5f;
            Item.DamageType = ScarletDamageClass.Instance;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = RarityType<TrueScarlet>();
            Item.noMelee = true;
            Item.channel = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<R99HeldProj>();
            //不要给这武器近程设计
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 0.1f, ProjectileType<R99HeldProj>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            string r99TypeName = GetInstance<R99>().GetType().Name;
            if (Item.type is ItemID.ChlorophyteBullet)
            {
                string path = CIFunction.GetTextValue($"Content.Items.Weapons.Ranged.{r99TypeName}.EnhancedBullet");
                FuckThisTooltipAndReplace(tooltips, path);
            }
        }
        /// <summary>
        /// 用替换的方法完全重写Tooltip
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="replacedTextPath"></param>
        public static void FuckThisTooltipAndReplace(List<TooltipLine> tooltips, string replacedTextPath)
        {
            tooltips.RemoveAll((line) => line.Mod == "Terraria" && line.Name != "Tooltip0" && line.Name.StartsWith("Tooltip"));
            TooltipLine getTooltip = tooltips.FirstOrDefault((x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (getTooltip is not null)
                getTooltip.Text = Language.GetTextValue(replacedTextPath);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Minigun>().
                AddIngredient<CelestialObliterator>().
                AddIngredient<DragonsBreathold>().
                AddIngredient<ExoPrism>(5).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();

        }
    }
}