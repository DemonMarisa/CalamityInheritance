using CalamityInheritance.Content.Projectiles.Melee.Flails;
using CalamityMod.Items;
using CalamityMod.Items.Placeables.Abyss;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Flails
{
    public class BallOFuguLegacy :GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 10;
            Item.damage = 40;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 8f;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ProjectileType<BallOFuguProjLegacy>();
            Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AbyssGravel>(8).
                AddIngredient<UrchinStinger>(6).
                AddIngredient(ItemID.Mace).
                AddIngredient(ItemID.Bone, 4).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
