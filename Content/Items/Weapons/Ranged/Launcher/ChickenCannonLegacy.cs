using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Ranged.Launcher;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Launcher
{
    public class ChickenCannonLegacy : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
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
            Item.rare = RarityType<YharonFire>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.shoot = ProjectileType<ChickenRound>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Rocket;

            Item.SetCalStatInflation(AllWeaponTier.PostDOG);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-15, 10);
    }
}