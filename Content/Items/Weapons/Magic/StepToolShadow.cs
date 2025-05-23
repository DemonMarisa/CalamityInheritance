using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Items.Placeables.Banners;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using CalamityInheritance.Rarity.Special;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class StepToolShadow : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 960;
            Item.height = 1120;
            Item.damage = 11451;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.mana = 15;
            Item.knockBack = 114f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.shootSpeed = 14f;
            Item.channel = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<StepToolShadowChair>();
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<TrueScarlet>() : ModContent.RarityType<PureRed>();
            Item.value = CIShopValue.RarityPricePureRed;
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
                AddIngredient(ItemID.Wood, 1451).
                AddIngredient<DepthCells>(41).
                AddIngredient<ReaperTooth>(9).
                AddIngredient<ReaperSharkBanner>(1).
                AddIngredient<CosmiliteBar>(9).
                AddIngredient<ShadowspecBar>(8).
                AddIngredient<DeepSeaDumbbell>(1).
                AddIngredient<Valediction>(1).
                AddIngredient<KnowledgeCalamitas>(1).
                AddTile(TileID.HeavyWorkBench).
                Register();
        }
    } 
}