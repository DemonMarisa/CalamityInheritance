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
    public class StellarContemptOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stellar Contempt");
            // Tooltip.SetDefault("Lunar flares rain down on enemy hits");
        }

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 74;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ModContent.ProjectileType<StellarContemptHammerOld>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FallenPaladinsHammer>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient(ItemID.FragmentSolar, 10).
                AddIngredient(ItemID.FragmentNebula, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
