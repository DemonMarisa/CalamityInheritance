using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class ApotheosisLegacy : CIMagic
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 77;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.width = 30;
            Item.height = 34;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = RarityType<DonatorPink>();
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ApothMarkLegacy>();
            Item.shootSpeed = 17f;
            Item.SetCustomMult_Int(778);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
