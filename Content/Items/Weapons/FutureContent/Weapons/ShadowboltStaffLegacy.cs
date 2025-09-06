using CalamityInheritance.Content.Projectiles.FutureContent.Shadowbolt;
using CalamityInheritance.Core;
using CalamityInheritance.Rarity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.FutureContent.Weapons
{
    public class ShadowboltStaffLegacy : CIMagic, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 56;
            Item.damage = 280;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShadowBeam>();
            Item.shootSpeed = 6f;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }
        public override void AddRecipes()
        {
            //尝试使用带防御性的编码
            int ruinousSoul = DefenseSystem.NameRuinSoul.TryGetItem();
            int armorShell = DefenseSystem.NameArmorShell.TryGetItem();
            CreateRecipe().
                AddIngredient(ItemID.ShadowbeamStaff).
                AddIngredient(armorShell, 3).
                AddIngredient(ruinousSoul, 2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}