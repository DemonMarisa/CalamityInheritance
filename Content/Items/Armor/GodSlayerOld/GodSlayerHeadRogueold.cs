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
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<GodSlayerChestplate>() && legs.type == ModContent.ItemType<GodSlayerLeggings>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

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
            const short onlyDash = 2;
            const short onlyReborn = 1;

            modPlayer.WearingPostMLSummonerSet = true;
            int mode = CIConfig.Instance.GodSlayerSetBonusesChange;
            modPlayer1.GodSlayerReborn = mode != onlyDash;
            player.setBonus = this.GetLocalizedValue("SetBonus") + "\n" + GodSlayerChestplateold.GetSpecial(mode);
            if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID && mode > onlyReborn)
            {
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }
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
