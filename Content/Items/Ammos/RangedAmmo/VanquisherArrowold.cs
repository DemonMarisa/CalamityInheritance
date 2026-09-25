using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Ammo.Ranged;
using CalamityInheritance.Content.Rarity;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Ammos.RangedAmmo
{
    public class VanquisherArrowold : CIAmmo
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 46;
            Item.damage = 33;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(silver: 32);
            Item.shoot = ProjectileType<VanquisherArrowoldMain>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
            Item.rare = RarityType<DeepBlue>();
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
