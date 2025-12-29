using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using static CalamityInheritance.Core.Enums;

namespace CalamityInheritance.Content.Items.Armor.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientGodSlayerHelm : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30; //130
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.ThisBodyPart<AncientGodSlayerChestplate>(ArmorPart.Body) && legs.ThisBodyPart<AncientGodSlayerLeggings>(ArmorPart.Legs); 

        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            calPlayer.wearingRogueArmor = true;
            calPlayer.WearingPostMLSummonerSet = true;
            calPlayer.godSlayer = true;
            calPlayer.rogueStealthMax += 1.25f;
            usPlayer.AncientGodSlayerSet = true;
            usPlayer.GodSlayerReborn = true;
            calPlayer.contactDamageReduction += 0.15f;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
            {
                calPlayer.DeferredDashID = GodslayerArmorDash.ID;
                if (usPlayer.AncinetGodSlayerDashReset)
                    calPlayer.rogueStealth = calPlayer.rogueStealthMax / 4 * 3;
                if (usPlayer.AncientGodSlayerBuffCD == 0)
                {
                    usPlayer.AncientGodSlayerBuffCounter = 600;
                    usPlayer.AncientGodSlayerBuffCD = 1500;
                }
                
                player.dash = 0;
                //旧套装通用新增；血上限，血药，回血
                calPlayer.healingPotionMultiplier += 0.70f;
                player.lifeRegen += 8; //+4HP/s
            }
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 5;
            player.maxTurrets += 3;
            player.statLifeMax2 += 200;
            player.GetDamage<GenericDamageClass>() += 0.40f;
            player.GetCritChance<GenericDamageClass>() += 25;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerHeadMeleeold>().
                AddIngredient<GodSlayerHeadRangedold>().
                AddIngredient<GodSlayerHeadRogueold>().
                AddIngredient<GodSlayerHeadMagicold>().
                AddIngredient<GodSlayerHeadSummonold>().
                AddIngredient<CosmiliteBar>(25).
                AddIngredient<AscendantSpiritEssence>(10).
                AddTile<CosmicAnvil>().
                Register();
                
        }
    }
}