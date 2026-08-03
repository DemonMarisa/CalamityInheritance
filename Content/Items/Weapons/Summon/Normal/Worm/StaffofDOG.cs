using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Summon.Normal.Worm;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Enums;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon.Normal.Worm
{
    public class StaffofDOG : CISummon
    {
        public static int BaseDamage = 150;
        public static int minionSlots = 3;
        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = BaseDamage;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 10; // 9 because of useStyle 1
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.UseSound = SoundID.Item113;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DOGworm>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            Item.LAP().UseCICalStatInflation = true;
            Item.LAP().WeaponTier = AllWeaponTier.PostDOG;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.maxMinions < 3)
                return false;
            if (player.maxMinions - player.slotsMinions < 3)
                return false;
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.active & p.type == ProjectileType<DOGworm>())
                    return false;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, 0, 1, player.whoAmI);
            Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 12).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}
