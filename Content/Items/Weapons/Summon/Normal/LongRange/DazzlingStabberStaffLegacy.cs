using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Summon.Normal.LongRange;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Summon.Normal.LongRange
{
    public class DazzlingStabberStaffLegacy : CISummon
    {
        public override void ExSD()
        {
            Item.width = 54;
            Item.height = 52;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_DarkMageHealImpact;
            Item.mana = 10;
            Item.damage = 127;
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 15;
            Item.shoot = ProjectileType<DazzlingStabberProj>();
            Item.shootSpeed = 13f;

            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;
            }
            float angleMax = MathHelper.ToRadians(45f);
            if (CIUtils.CountProjectiles(type) == 1)
                angleMax = 0f;
            float index = 1f;
            if (player.ownedProjectileCounts[Item.shoot] > 8)
            {
                angleMax += MathHelper.ToRadians((player.ownedProjectileCounts[Item.shoot] - 8) * 2.5f);
            }
            angleMax = angleMax > MathHelper.ToRadians(105f) ? MathHelper.ToRadians(105f) : angleMax; // More intuative than using a min function
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == type && p.owner == player.whoAmI)
                {
                    p.ai[1] = index / CIUtils.CountProjectiles(type) * angleMax - angleMax / 2f;
                    p.netUpdate = true;
                    index++;
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.UnholyEssence, 9).
                    AddIngredient(CalamityMaterials.DivineGeode, 5).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
