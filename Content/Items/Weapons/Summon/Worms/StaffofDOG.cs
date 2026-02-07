using CalamityInheritance.Content.Projectiles.Summon.Worms;
using CalamityInheritance.Rarity;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Core.Enums;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Summon.Worms
{
    public class StaffofDOG : CISummon, ILocalizedModType
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
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
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
            if ((player.maxMinions - player.slotsMinions) < 3)
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
            CreateRecipe().
                AddIngredient<CosmiliteBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
