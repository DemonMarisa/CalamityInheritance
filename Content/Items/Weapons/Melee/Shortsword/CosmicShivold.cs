using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class CosmicShivold : CIMelee
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.width = 44;
            Item.height = 44;
            Item.damage = 148;
            Item.knockBack = 9f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<CosmicShivProjold>();
            Item.shootSpeed = 2.4f;

            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
        }

        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<ElementalShivold>().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 8).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}
