using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.Magic;
using Terraria.GameContent.Golf;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class PhantasmalFuryOld : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<PhantasmalFury>(false);
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
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.DamageType = DamageClass.Ranged; 
                Item.mana = 0;
                Item.useTime =  15;
                Item.useAnimation = 15;
            }
            else
            {
                Item.DamageType = DamageClass.Magic;
                Item.mana = 20;
                Item.useTime = 20;
                Item.useAnimation = 20;
            }
            return base.CanUseItem(player);
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
            CreateRecipe().
                AddIngredient(ItemID.SpectreStaff).
                AddIngredient<RuinousSoul>(2).
                AddIngredient<DarkPlasma>().
                AddCondition(Condition.NotZenithWorld).
                AddDecraftCondition(Condition.NotZenithWorld).
                AddTile(TileID.LunarCraftingStation).
                Register();
                
            CreateRecipe().
                AddIngredient(ItemID.SpectreStaff).
                AddIngredient<CoreofEleum>(3).
                AddCondition(Condition.ZenithWorld).
                AddDecraftCondition(Condition.ZenithWorld).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
