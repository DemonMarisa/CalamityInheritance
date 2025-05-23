using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Armor.Astral;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAstral
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAstralHelm: CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare =  ItemRarityID.Red;
            Item.defense = 22;
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 20;
            player.GetCritChance<RogueDamageClass>() += 5;
            player.lifeRegen += 1;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AncientAstralBreastplate>() && legs.type == ModContent.ItemType<AncientAstralLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer calPlayer = player.Calamity();
            var usPlayer = player.CIMod(); 
            player.statLifeMax2 += 40;
            player.lifeRegen += 1;
            player.pStone = true;
            calPlayer.wearingRogueArmor = true;
            calPlayer.rogueStealthMax += 1.15f;
            calPlayer.stealthStrikeHalfCost = true;
            player.endurance += 0.12f;
            usPlayer.AncientAstralSet = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");

            player.Calamity().WearingPostMLSummonerSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<StarblightSoot>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}