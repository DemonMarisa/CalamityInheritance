using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Buffs.DamageOverTime;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.Weapons.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class StepToolShadow : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";


        public override void SetDefaults()
        {
            Item.width = 960;
            Item.height = 1120;
            Item.damage = 1500;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.mana = 15;
            Item.knockBack = 114f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 14f;
            Item.channel = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<StepToolShadowChair>();
            Item.rare = ModContent.RarityType<PureRed>();
            Item.value = CIShopValue.RarityPricePureRed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PortableStool, 5).
                AddIngredient(ItemID.Wood, 100).
                AddIngredient<ReaperTooth>(50).
                AddIngredient<DepthCells>(50).
                AddIngredient<CosmiliteBar>(50).
                AddIngredient<ShadowspecBar>(10).
                AddIngredient<Valediction>(1).
                AddIngredient<DeepSeaDumbbell>(1).
                AddIngredient<KnowledgeCalamitas>(1).
                AddTile(TileID.HeavyWorkBench).
                Register();
        }
    } 
}