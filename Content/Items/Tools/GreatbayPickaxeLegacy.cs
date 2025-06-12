using CalamityInheritance.Content.Items.Weapons.Ranged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Tools
{
    public class GreatbayPickaxeLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.knockBack = 2f;
            Item.useTime = 8;
            Item.useAnimation = 16;
            Item.pick = 65;

            Item.DamageType = DamageClass.Melee;
            Item.width = 44;
            Item.height = 44;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
