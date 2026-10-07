using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.RogueMelee;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.RogueMelee
{
    public class NanoblackReaper : CIMeleeRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            EffectColor = Color.DarkBlue;

            Item.width = 78;
            Item.height = 64;
            Item.damage = 180;
            Item.knockBack = 9f;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item18;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = RarityType<DonatorPink>();

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.ArmorPenetration = 500;
            Item.shoot = ProjectileType<NanoblackReaperProj>();
            Item.shootSpeed = 12;

            Item.SetCalStatInflation(AllWeaponTier.DemonShadow);
        }
        public override bool CanUseWeaponSkill(Player player)
        {
            return false;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                Register();
        }
    }
}
