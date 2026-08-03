using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Summon.Header;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Summon
{
    public class MountedScannerLegacy : CISummon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.DamageType = DamageClass.Summon;
            Item.damage = Main.zenithWorld ? 420 : 42;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 24;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item15;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = RarityType<OrangeDraedon>();

            Item.shoot = ProjectileType<MountedScannerSummonLegacy>();
            Item.shootSpeed = 1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var scanner = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
            scanner.originalDamage = Item.damage;

            int totalOwnedScanners = player.ownedProjectileCounts[type];
            int currentScannerIndex = 0;
            foreach (Projectile projectile in Main.projectile)
            {
                if (!projectile.active)
                    continue;
                if (projectile.type != type)
                    continue;
                if (projectile.owner != player.whoAmI)
                    continue;
                float completionRatio = currentScannerIndex / (float)totalOwnedScanners;

                // ai[0] is the angular offset relative to the projectile's owner.
                // For the first 15 summons, wrap around the player angularly, but not at a perfect angle, a bit like the Dazzling Stabbers when idle.
                // But once the total summon count is greater than 15, just create a perfect circle depending on the total amount of summons.
                if (totalOwnedScanners <= 14)
                {
                    projectile.ai[0] = 0f.AngleLerp(MathHelper.Pi, currentScannerIndex / 15f);
                    if (currentScannerIndex % 2f == 1f)
                        projectile.ai[0] = -0f.AngleLerp(MathHelper.Pi, (currentScannerIndex + 1) / 15f);
                }
                else
                {
                    projectile.ai[0] = MathHelper.TwoPi / totalOwnedScanners * currentScannerIndex;
                }

                // Add a specific offset so that the scanners spawn above the player at first and not to the side.
                projectile.ai[0] -= MathHelper.PiOver2;
                projectile.netUpdate = true;
                currentScannerIndex++;
            }
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 8).
                    AddIngredient(CalamityMaterials.DubiousPlating, 12).
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofSight, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                    AddIngredient(ItemID.SoulofSight, 20).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
