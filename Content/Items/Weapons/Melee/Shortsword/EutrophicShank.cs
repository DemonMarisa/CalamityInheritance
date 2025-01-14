using System.Security.Cryptography.Xml;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class EutrophicShank : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 14;
            Item.width = 42;
            Item.height = 38;
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<EutrophicShankProj>();
            Item.shootSpeed = 2.8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
    }
}
