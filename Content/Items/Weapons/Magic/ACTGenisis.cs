using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class ACTGenisis : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<GenisisLegacy>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 28;
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 25;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare =  CIConfig.Instance.SpecialRarityColor? RarityType<AlgtPink>(): ItemRarityID.Purple;
            Item.UseSound = CISoundMenu.GenisisFire;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ProjectileType<ACTGenisisHeldProj>();

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<ACTGenisisHeldProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<ACTGenisisHeldProj>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LaserMachinegun).
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
