using CalamityInheritance.Content.Projectiles.Magic.Staffs;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Staffs
{
    public class VisceraLegacy : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public const int BoomLifetime = 40;
        public int Counter = 0;
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VisceraBeamLegacy>();
            Item.shootSpeed = 6f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(4).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
