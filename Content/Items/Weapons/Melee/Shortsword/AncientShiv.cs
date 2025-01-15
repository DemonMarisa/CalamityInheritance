using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class AncientShiv : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.33f;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 12;
            Item.width = 30;
            Item.height = 30;
            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<AncientShivProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
    }
}
