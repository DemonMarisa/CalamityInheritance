using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class HeartRapier : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 20;
            Item.width = 64;
            Item.height = 64;
            Item.damage = 38;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<HeartRapierProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LifeCrystal, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
