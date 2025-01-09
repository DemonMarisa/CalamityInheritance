using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using log4net.Core;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AstralArcanum : ModItem
    {

        public override void SetDefaults()
        {
            Item.defense = 12;
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityInheritanceKeybinds.AstralArcanumUIHotkey);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            modPlayer1.astralArcanum = true;
            modPlayer1.projRef = true;
            player.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<InfectedJewel>().
                AddIngredient<ArcanumoftheVoid>().
                AddIngredient<AstralBulwark>().
                AddIngredient<DarkPlasma>(3).
                AddIngredient(ItemID.LunarBar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
