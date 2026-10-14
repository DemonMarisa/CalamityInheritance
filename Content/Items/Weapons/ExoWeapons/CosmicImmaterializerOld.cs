using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Summon.Normal.CloseRange;
using CalamityInheritance.Content.Items.Weapons.Summon.Normal.Limits;
using CalamityInheritance.Content.Projectiles.ExoWeapons.CosmicSpiral;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class CosmicImmaterializerOld : CIExoWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.damage = 1000;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 74;
            Item.height = 72;
            Item.useTime = Item.useAnimation = 10;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.UseSound = SoundID.Item60;
            Item.shoot = ProjectileType<CosmicEnergySpiralOld>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.rare = RarityType<CatalystViolet>();
        }
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && player.maxMinions >= 10;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int x = 0; x < Main.maxProjectiles; x++)
            {
                Projectile proj = Main.projectile[x];
                if (proj.active && proj.owner == player.whoAmI && proj.type == type)
                {
                    proj.Kill();
                    break;
                }
            }
            int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SiriusLegacy>().
                AddIngredient<AncientAncientIceChunk>().
                AddIngredient<AuricBarold>(15).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<SiriusLegacy>().
                AddIngredient<AncientAncientIceChunk>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
