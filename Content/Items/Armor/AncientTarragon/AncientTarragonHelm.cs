using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientTarragon
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientTarragonHelm : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 20; //90
        }
        

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AncientTarragonBreastplate>() && legs.type == ModContent.ItemType<AncientTarragonLeggings>();
        }
        
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            var calPlayer = player.Calamity();
            usPlayer.AncientTarragonSet = true;
            calPlayer.rogueStealthMax += 1.20f;
            calPlayer.wearingRogueArmor = true;
            calPlayer.WearingPostMLSummonerSet = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }
        
        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.05f;
            player.maxMinions += 3;
            player.statLifeMax2 += 150;
            player.lavaImmune = true;
            player.ignoreWater = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TarragonHeadMelee>().
                AddIngredient<TarragonHeadRanged>().
                AddIngredient<TarragonHeadMagic>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<UelibloomBar>(15).
                AddIngredient<DivineGeode>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}