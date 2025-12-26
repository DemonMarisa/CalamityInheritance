using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Rarity;
using CalamityMod.Sounds;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Summon;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class YharonSonStaff: CISummon, ILocalizedModType
    {
        public static readonly int WeaponDamage = 160;
        public override void SetStaticDefaults()
        {
            //改为法杖的形式会更加符合这个武器的设计
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<YharonsKindleStaff>();
        }
        public override void SetDefaults()
        {
            Item.height = 48;    
            Item.width = 56;
            Item.mana = 50;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.DamageType = DamageClass.Summon;
            Item.damage = WeaponDamage;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CommonCalamitySounds.FlareSound;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<YharonFire>() :ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ModContent.ProjectileType<SonYharon>();
            Item.shootSpeed = 10f;
        }
    }
}