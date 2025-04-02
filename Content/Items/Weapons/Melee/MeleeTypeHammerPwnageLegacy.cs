using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class MeleeTypeHammerPwnageLegacy : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory =>$"{Generic.WeaponLocal}.Melee";
        public static readonly float Speed = 12f;
        public override void SetStaticDefaults()
        {
            if(CIServerConfig.Instance.CustomShimmer == true)
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Pwnagehammer>()] = ModContent.ItemType<MeleeTypeHammerPwnageLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = ModContent.ItemType<Pwnagehammer>();
            }
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = 90;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.knockBack = 10f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>();
            Item.shootSpeed = 12f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Pwnhammer).
                AddIngredient(ItemID.SoulofFright, 3).
                AddIngredient(ItemID.SoulofMight, 3).
                AddIngredient(ItemID.SoulofSight, 3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}