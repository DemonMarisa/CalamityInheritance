using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic.GreatStaff;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.GreatStaff
{
    public class PhantasmalFuryOld : CIMagic
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 60;
            Item.damage = 420;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PhantasmalFuryProjOld>();
            Item.shootSpeed = 12f;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (Main.zenithWorld)
            {
                damage.Base = 100;
            }
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.SpectreStaff).
                    AddIngredient(CalamityMaterials.RuinousSoul, 2).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
