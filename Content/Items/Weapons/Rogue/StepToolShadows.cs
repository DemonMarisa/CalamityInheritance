using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Items.Placeables.Banners;
using System.Collections.Generic;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class StepToolShadows : CIRogueClass
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 960;
            Item.height = 1120;
            Item.damage = 1145;
            Item.DamageType = GetInstance<RogueDamageClass>();
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 114f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.shootSpeed = 14f;
            Item.channel = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<StepToolShadowChair>();
            Item.rare = RarityType<PureRed>();
            Item.value = CIShopValue.RarityPricePureRed;
        }

        public override float StealthDamageMultiplier => 1145.14f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs("Alt");
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool isHoldingAlt = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt);
            string defaultPath = $"{Generic.WeaponTextPath}Rogue.{GetType().Name}.Lore";
            if (isHoldingAlt)
                tooltips.FuckThisTooltipAndReplace(defaultPath);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float getTrueMeleeBoost = player.GetTotalDamage<TrueMeleeDamageClass>().ApplyTo(Item.damage); //梯凳现在可以吃到真近战伤害加成
            Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, (int)getTrueMeleeBoost, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PortableStool, 1).
                AddIngredient(ItemID.Wood, 1145).
                AddIngredient<ReaperTooth>(14).
                AddIngredient<CosmiliteBar>(19).
                AddIngredient<DeepSeaDumbbell>(19).
                AddIngredient<ReaperSharkBanner>().
                AddIngredient<CalamitousEssence>().
                AddIngredient<KnowledgeCalamitas>().
                AddTile<DemonshadeTile>().
                Register();
        }
    } 
}