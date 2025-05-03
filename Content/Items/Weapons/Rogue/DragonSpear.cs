using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Build.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class DragonSpear : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<Wrathwing>();
        }
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 400;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<YharonFire>() : ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.shoot = ModContent.ProjectileType<DragonSpearProj>();
            Item.shootSpeed = 28f;
        }
    }
}
