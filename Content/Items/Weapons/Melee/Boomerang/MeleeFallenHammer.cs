using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class MeleeFallenHammer : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<FallenPaladinsHammer>();
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<MeleeFallenHammerProj>();
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<MeleePwnagehammer>(). //改成旧锤子
                AddIngredient<ScoriaBar>(5).
                AddIngredient<AshesofCalamity>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
