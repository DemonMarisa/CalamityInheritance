using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeFallenPaladinsHammer : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<FallenPaladinsHammerProjRogue>();
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<Pwnagehammer>().
                AddIngredient<ScoriaBar>(5).
                AddIngredient<AshesofCalamity>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
