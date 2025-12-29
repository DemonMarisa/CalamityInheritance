using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ChickenCannonLegacy : CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<ChickenCannon>();
        }
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
            Item.rare = CIConfig.Instance.SpecialRarityColor ? RarityType<YharonFire>() : RarityType<DeepBlue>();
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.shoot = ProjectileType<ChickenRound>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Rocket;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-15, 10);
    }
}