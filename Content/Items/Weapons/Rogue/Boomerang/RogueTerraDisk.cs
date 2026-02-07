using CalamityInheritance.Content.Items.Weapons.Melee.Boomerang;
using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class RogueTerraDisk : CIRogueClass
    {
        public override string Texture => GetInstance<MeleeTerraDisk>().Texture;
        public static float Speed = 12f;

        public override void ExSD()
        {
            Item.width = 60;
            Item.height = 64;
            Item.damage = 100;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 30;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ProjectileType<RogueTerraDiskProj>();
            Item.shootSpeed = Speed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EquanimityLegacy>().
                AddIngredient<Brimblade>().
                AddIngredient<LivingShard>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
