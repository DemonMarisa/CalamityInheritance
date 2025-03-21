using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadRogue : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30; //96
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isSilvaSetNEW = body.type == ModContent.ItemType<SilvaArmor>() && legs.type == ModContent.ItemType<SilvaLeggings>();
            bool isSilvaSetOLD = body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
            return isSilvaSetNEW || isSilvaSetOLD;
        }
        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CIMod();
            var modPlayer = player.Calamity();
            modPlayer1.AuricSilvaSet = true;
            modPlayer1.SilvaRougeSetLegacy = true;
            modPlayer1.SilvaRebornMark = true;
            modPlayer.rogueStealthMax += 1.25f;
            modPlayer.wearingRogueArmor = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (player.statLife > (int)(player.statLifeMax2 * 0.5) && player.HeldItem.DamageType == ModContent.GetInstance<RogueDamageClass>() && player.HeldItem.useTime > 3)
            {
                player.GetAttackSpeed<RogueDamageClass>() += 0.1f;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ThrowingDamageClass>() += 0.13f;
            player.GetCritChance<ThrowingDamageClass>() += 13;
            player.moveSpeed += 0.2f;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(6).
                AddIngredient<EffulgentFeather>(5).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
