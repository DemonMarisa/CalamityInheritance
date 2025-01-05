using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadRogue : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Vanity";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 30; //96
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isSilvaSetNEW = body.type == ModContent.ItemType<SilvaArmor>() && legs.type == ModContent.ItemType<SilvaLeggings>();
            bool isSilvaSetOLD = body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
            return isSilvaSetNEW || isSilvaSetOLD;
        }
        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CalamityInheritance();
            var modPlayer = player.Calamity();
            modPlayer1.auricsilvaset = true;
            modPlayer1.silvaRogue = true;
            modPlayer1.silvaRebornMark = true;
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
    }
}
