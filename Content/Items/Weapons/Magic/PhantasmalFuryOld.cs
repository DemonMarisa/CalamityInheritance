using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.Magic;
using Terraria.GameContent.Golf;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class PhantasmalFuryOld : CIMagic, ILocalizedModType
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
            Item.damage = 260;
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
            Item.shoot = ModContent.ProjectileType<PhantasmalFuryProjOld>();
            Item.shootSpeed = 12f;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.damage =  100;
                Item.DamageType = DamageClass.Ranged; 
                Item.mana = 0;
                Item.useTime =  15;
                Item.useAnimation = 15;
            }
            else
            {
                Item.damage = 295;
                Item.DamageType = DamageClass.Magic;
                Item.mana = 20;
                Item.useTime = 20;
                Item.useAnimation = 20;
            }
            return base.CanUseItem(player);
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
