using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Ammo.Ranged;

namespace CalamityInheritance.Content.Items.Ammos.RangedAmmo
{
    public class GodSlayerSlugLegacy : CIAmmo
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(copper: 28);
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shoot = ModContent.ProjectileType<GodSlayerSlugProj>();
            Item.shootSpeed = 6f;
            Item.ammo = ItemID.MusketBall;
        }

        public override void AddRecipes()
        {
            if (HasCalamity())
            {
                CreateRecipe(999).
                    AddIngredient(ItemID.EmptyBullet, 999).
                    AddIngredient(CalamityMaterials.CosmiliteBar).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
        }
    }
}
