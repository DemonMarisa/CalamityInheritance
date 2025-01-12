using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatBuffs; 
using CalamityMod.CalPlayer;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientXerocMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Xeroc Mask");
            /* Tooltip.SetDefault("11% increased rogue damage and critical strike chance\n" +
                "Immune to lava, cursed, fire, cursed inferno, and chilled\n" +
                "Wrath of the cosmos"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 20; //71
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AncientXerocPlateMail>() && legs.type == ModContent.ItemType<AncientXerocCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            modPlayer.wearingRogueArmor = true;
            modPlayer.xerocSet = true;
            modPlayer1.AncientXerocMadness = true;
            modPlayer.rogueStealthMax += 1.15f;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if(player.statLife<=(player.statLifeMax2 * 0.8f) && player.statLife > (player.statLifeMax2 * 0.6f))
            {
                player.manaCost *= 0.95f; 
                player.GetDamage<GenericDamageClass>() +=0.05f;
                player.GetCritChance<GenericDamageClass>() += 5;
            }
            else if(player.statLife<=(player.statLifeMax2 * 0.6f) && player.statLife > (player.statLifeMax2 * 0.4f))
            {
                player.manaCost *= 0.9f; 
                player.GetDamage<GenericDamageClass>() += 0.10f;
                player.GetCritChance<GenericDamageClass>() += 10;
            }
            else if(player.statLife<=(player.statLifeMax2 * 0.4f) && player.statLife > (player.statLifeMax2 * 0.2f))
            {
                player.manaCost *= 0.80f;
                player.GetDamage<GenericDamageClass>() += 0.20f;
                player.GetCritChance<GenericDamageClass>() += 20;
            }
            player.GetDamage<ThrowingDamageClass>() += 0.09f;
            modPlayer.rogueVelocity += 0.09f;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
            player.GetDamage<GenericDamageClass>()     += 0.1f;
            player.GetCritChance<GenericDamageClass>() += 10;
            player.lavaImmune = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Chilled] = true;
        }
    }
}
