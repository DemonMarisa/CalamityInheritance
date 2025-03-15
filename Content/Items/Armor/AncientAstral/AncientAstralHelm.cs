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
    public class AncientAstralHelm: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            if(CalamityConditions.DownedAstrumDeus.IsMet())
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AncientAstralHelm>()] = ModContent.ItemType<AstralHelm>();
            }
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
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool ifArmor = body.type == ModContent.ItemType<AncientAstralBreastplate>() && legs.type == ModContent.ItemType<AncientAstralLeggings>();
            return ifArmor;
        }

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
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<StarblightSoot>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

    }
}