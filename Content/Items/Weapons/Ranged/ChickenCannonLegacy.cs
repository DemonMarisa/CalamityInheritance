using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ChickenCannonLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 50;
            Item.ArmorPenetration = 50;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundID.SoundShoutgunTactical;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<YharonFire>() :ModContent.RarityType<DeepBlue>();
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.shoot = ModContent.ProjectileType<ChickenRound>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Rocket;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-15, 10);
    }
}