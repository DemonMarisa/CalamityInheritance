using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("EradicatorLegacyMelee")]
    public class MeleeTypeEradicator : ModItem, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static float Speed = 9.0f;
        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //Scarlet:微光启用后才后允许互转
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Eradicator>()] = ModContent.ItemType<MeleeTypeEradicator>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeEradicator>()] = ModContent.ItemType<Eradicator>();
            }
        }

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
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shoot = ModContent.ProjectileType<MeleeTypeEradicatorProj>();
            Item.shootSpeed = Speed;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeEradicatorGlow.png").Value);
        }
    }
}
