using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Content.Items.Armor.AncientBloodflare;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.Content.Items.Armor.AncientSilva;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Armor.AncientAuric
{
    [AutoloadEquip(EquipType.Body)]
    public class YharimAuricTeslaBodyArmor : CIArmor, ILocalizedModType
    {
        public override void Load()
        {
            // All code below runs only if we're not loading on a server
            if (Main.netMode == NetmodeID.Server)
                return;
            // Add equip textures
            EquipLoader.AddEquipTexture(Mod, "CalamityInheritance/Content/Items/Armor/AncientAuric/YharimAuricTeslaBodyArmor_Back", EquipType.Back, this);
        }
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
            Item.defense = 110;
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 1000;
            player.statManaMax2 += 500;
            player.moveSpeed += 0.25f;
        }
    }
}