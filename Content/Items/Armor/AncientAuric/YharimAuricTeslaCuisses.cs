using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Content.Items.Armor.AncientBloodflare;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.Content.Items.Armor.AncientSilva;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Armor.AncientAuric
{
    [AutoloadEquip(EquipType.Legs)]
    public class YharimAuricTeslaCuisses : CIArmor, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.5f;
			player.statLifeMax2 += 600;
            player.carpet = true;
        }
    }
}