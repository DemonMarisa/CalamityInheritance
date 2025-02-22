using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("ElementalDiskLegacyMelee")]
    public class MeleeTypeElementalDisk : ModItem, ILocalizedModType 
    {

        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.damage = 52; //每个分裂的飞盘现在都采用8独立无敌帧，因此面板大动干戈直接拉成两位数。总体来说dps属于正常水平。
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ModContent.ProjectileType<MeleeTypeElementalDiskProj>();
            Item.shootSpeed = 13f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeleeTypeMangroveChakram>().
                AddIngredient<MeleeTypeSubductionSlicer>().
                AddIngredient<MeleeTypeTerraDisk>().
                AddIngredient<GalacticaSingularity>(5).
                AddIngredient<LifeAlloy>(5).
                AddIngredient(ItemID.LunarBar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
