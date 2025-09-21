using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AstralArcanum : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:26,
            itemHeight:26,
            itemRare:ModContent.RarityType<BlueGreen>(),
            itemValue:CIShopValue.RarityPriceBlueGreen,
            itemDefense:12
        );
        public override void ExSSD() => Type.ShimmerEach<Radiance>();
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityInheritanceKeybinds.AstralArcanumUIHotkey);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.AstralBulwark = true;
            usPlayer.AstralArcanumEffect = true;
            usPlayer.projRef = true;
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
