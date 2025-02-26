using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("CorpusAvertorLegacyMelee")]
    public class MeleeTypeCorpusAvertor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //Scarlet:微光启用后才后允许互转
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeCorpusAvertor>()] = ModContent.ItemType<CorpusAvertor>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<CorpusAvertor>()] = ModContent.ItemType<MeleeTypeCorpusAvertor>();
            }
        }
        public override void SetDefaults()
        {
            //改动：采用1457的面板，且加强了追踪性能
            Item.width = 32;
            Item.height = 44;
            Item.damage = 140;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<MeleeTypeCorpusAvertorProj>();
            Item.shootSpeed = 8.5f;
            Item.DamageType = DamageClass.Melee;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int lifeAmount = player.statLifeMax2 - player.statLife;
            damage.Base += lifeAmount * 0.1f;
        }
    }
}
