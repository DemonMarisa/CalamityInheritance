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
using CalamityMod;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeStellarContempt : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 74;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
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

            Item.shoot = ModContent.ProjectileType<StellarContemptHammerRogue>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {

            if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
            {
            CreateRecipe().
                AddIngredient<FallenPaladinsHammer>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient(ItemID.FragmentSolar, 10).
                AddIngredient(ItemID.FragmentNebula, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
            }


                CreateRecipe().
                    AddIngredient<RogueTypeFallenPaladinsHammer>().
                    AddIngredient(ItemID.LunarBar, 5).
                    AddIngredient(ItemID.FragmentSolar, 10).
                    AddIngredient(ItemID.FragmentNebula, 10).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
        }
    }
}
