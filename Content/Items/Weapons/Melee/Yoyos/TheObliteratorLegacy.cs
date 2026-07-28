using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Yoyos;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Yoyos
{
    public class TheObliteratorLegacy : CIMelee
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 40;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 222;
            Item.knockBack = 7.5f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<ObliteratorYoyoLegacy>();
            Item.shootSpeed = 16f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient(CalamityMaterials.CosmiliteBar, 8)
                    .AddTile(CalamityTile.CosmicAnvilTile)
                    .Register();
            }
            else
            {

            }
        }
    }
}
