using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Content.Projectiles.Typeless.Shizuku;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem
{
    public class ShizukuEdge : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Typeless";
        public static readonly SoundStyle ProjectileDeathSound = SoundID.NPCDeath39 with { Volume = 0.5f };
        public static readonly int BaseDamage = 15000;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 108;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Generic;
            Item.useStyle = ItemUseStyleID.HoldUp;
            //it dosen't matter.
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.knockBack = 5.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;

            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<ShizukuAqua>();

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useTurn = true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ShizukuEdgeSword>()] < 1 || player.altFunctionUse == 2;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projLeft = ModContent.ProjectileType<ShizukuEdgeSword>();
            int projRight = ModContent.ProjectileType<ShizukuEdgeMoon>();
            
            if (player.ownedProjectileCounts[projRight] < 1 && player.altFunctionUse == 2)
                Projectile.NewProjectile(source, new Vector2(player.Center.X, player.Center.Y - 24f), velocity, projRight, damage, knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, position, velocity, projLeft, damage, knockback, player.whoAmI);
            return false;
        }
        // public override void AddRecipes()
        // {
        //     CreateRecipe().
        //             AddRecipeGroup(CIRecipeGroup.AnyMoonMusicBox).
        //             AddRecipeGroup(CIRecipeGroup.AnyRareReaper).
        //             AddRecipeGroup(CIRecipeGroup.AnySoulEdge).
        //             AddRecipeGroup(CIRecipeGroup.AnyChest).
        //             AddIngredient<CosmiliteBar>(10).
        //             AddIngredient(ItemID.LunarBar, 20).
        //             AddIngredient<ShadowspecBar>(5).
        //             AddIngredient<Lumenyl>(30).
        //             AddCondition(Condition.NearShimmer).
        //             Register();

        // }
    }
}