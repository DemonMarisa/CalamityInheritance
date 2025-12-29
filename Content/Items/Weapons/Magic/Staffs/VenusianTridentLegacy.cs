using CalamityInheritance.Content.Projectiles.Magic.Staffs;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Staffs
{
    public class VenusianTridentLegacy : CIMagic, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 68;
            Item.damage = 256;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item45;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<VenusianBoltLegacy>();
            Item.shootSpeed = 19f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.InfernoFork).
                AddIngredient<RuinousSoul>(2).
                AddIngredient<TwistingNether>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
