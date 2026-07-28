using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.GreatStaff;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.GreatStaff
{
    public class TheWandold : CIMagic
    {

        // The actual base damage of The Wand. The damage reported on the item is just the spark, which is irrelevant.
        public static int BaseDamage = 999;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 36;
            Item.damage = 1;
            Item.mana = 150;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.useAnimation = 250;
            Item.useTime = 250;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.5f;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ProjectileType<SparkInfernalold>();
            Item.shootSpeed = 24f;
            Item.rare = RarityType<CatalystViolet>();
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(10, 10);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch);
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.WandofSparking).
                    AddIngredient(CalamityMaterials.YharonSoulFragment, 8).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}
