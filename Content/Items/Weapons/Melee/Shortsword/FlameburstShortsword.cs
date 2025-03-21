using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class FlameburstShortsword : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.33f;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 36;
            Item.height = 38;
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<FlameburstShortswordProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellstoneBar, 7).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
