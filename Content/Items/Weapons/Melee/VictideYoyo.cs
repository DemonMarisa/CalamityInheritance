using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class VictideYoyo: CIMelee, ILocalizedModType 
    {
        
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 13;
            Item.knockBack = 3f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<VictideYoyoProj>();
            Item.shootSpeed = 10f;

            Item.rare = ItemRarityID.Green;
            Item.value = CIShopValue.RarityPriceGreen;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodYoyo, 1).
                AddIngredient<AncientVictideBar>(5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
