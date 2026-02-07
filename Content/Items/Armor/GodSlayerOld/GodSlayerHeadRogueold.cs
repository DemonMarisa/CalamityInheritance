using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadRogueold : CIArmor, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 29; //96
            Item.rare = RarityType<DeepBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<GodSlayerChestplateold>() && legs.type == ItemType<GodSlayerLeggingsold>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityKeybinds.GodSlayerDashHotKey);
        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            var modPlayer = player.Calamity();

            modPlayer.godSlayer = true;
            modPlayer.godSlayerThrowing = true;
            modPlayer1.GodSlayerRogueSet = true;
            modPlayer.rogueStealthMax += 1.4f;
            //根据潜伏值上限获得更多的潜伏值增益
            float getMaxStealth = modPlayer.rogueStealthMax;
            modPlayer.rogueStealthMax += getMaxStealth / 7;
            modPlayer.wearingRogueArmor = true;

            modPlayer.WearingPostMLSummonerSet = true;

            modPlayer1.GodSlayerReborn = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            modPlayer1.CanUseLegacyGodSlayerDash = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RogueDamageClass>() += 0.14f;
            player.GetCritChance<RogueDamageClass>() += 14;
            player.moveSpeed += 0.18f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(7).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
