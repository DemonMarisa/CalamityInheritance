using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class FallenPaladinsHammerOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fallen Paladin's Hammer");
            // Tooltip.SetDefault("Explodes on enemy hits");
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
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<FallenPaladinsHammerProjOld>();
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
