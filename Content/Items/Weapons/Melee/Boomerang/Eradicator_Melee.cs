using CalamityInheritance.Content.Projectiles.Melee.Boomerang;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class Eradicator_Melee : CIMelee, ILocalizedModType
    {
        public static float Speed = 9.0f;
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 58;
            Item.damage = 100;
            //降低飞盘伤害，提高星云射线的倍率（0.4→0.8），且极大程度地提高了星云射线的索敌范围与蛇毒，与发射频率
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.knockBack = 7f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shoot = ModContent.ProjectileType<MeleeTypeEradicatorProj>();
            Item.shootSpeed = Speed;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CosmiliteBar>(12)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
