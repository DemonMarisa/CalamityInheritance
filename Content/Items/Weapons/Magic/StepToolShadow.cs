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
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items.Placeables.Banners;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class StepToolShadow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public int NewDamage =  CIServerConfig.Instance.ShadowspecBuff? 11451 : 1145;
        public override void SetDefaults()
        {
            Item.width = 960;
            Item.height = 1120;
            Item.damage = NewDamage;
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
            Item.rare = ModContent.RarityType<PureRed>();
            Item.value = CIShopValue.RarityPricePureRed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float getTrueMeleeBoost = player.GetTotalDamage<TrueMeleeDamageClass>().ApplyTo(Item.damage); //梯凳现在可以吃到真近战伤害加成
            Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, (int)getTrueMeleeBoost, knockback, player.whoAmI);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine newLore = new(Mod, "CalamityMMod:Lore", this.GetLocalizedValue("StelStoolLore"));
            newLore.OverrideColor = Color.LightYellow;
            CalamityUtils.HoldShiftTooltip(tooltips, [newLore], true);
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