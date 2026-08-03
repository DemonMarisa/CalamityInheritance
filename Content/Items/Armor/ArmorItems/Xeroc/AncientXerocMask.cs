using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Armor.Rogue;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.Xeroc
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientXerocMask : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 10; //50
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientXerocPlateMail>() && legs.type == ItemType<AncientXerocCuisses>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.SetRogueArmor(1.1f, true);
            player.setBonus = this.GetLocalizedValue("SetBonus");
            XerocSetbouns(player);
        }
        public static void XerocSetbouns(Player player)
        {
            if (player.statLife <= player.statLifeMax2 * 0.8f && player.statLife > player.statLifeMax2 * 0.6f)
            {
                player.GetDamage<GenericDamageClass>() += 0.10f;
                player.GetCritChance<GenericDamageClass>() += 10;
            }

            else if (player.statLife <= player.statLifeMax2 * 0.6f && player.statLife > player.statLifeMax2 * 0.25f)
            {
                player.GetDamage<GenericDamageClass>() += 0.15f;
                player.GetCritChance<GenericDamageClass>() += 15;
            }

            else if (player.statLife <= player.statLifeMax2 * 0.25f && player.statLife > player.statLifeMax2 * 0.15f)
            {
                player.AddBuff(BuffType<AncientXerocMadness>(), 2);
                player.GetDamage<GenericDamageClass>() += 0.40f;
                player.GetCritChance<GenericDamageClass>() += 40;
                player.manaCost *= 0.10f;
                player.LAP().healingPotionMult += 0.10f;
            }
            else if (player.statLife <= player.statLifeMax2 * 0.15f)
            {
                player.AddBuff(BuffType<AncientXerocShame>(), 2);
                player.GetDamage<GenericDamageClass>() -= 0.40f;
                player.GetCritChance<GenericDamageClass>() -= 40;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.05f;
            player.GetCritChance<GenericDamageClass>() += 5;
            player.lavaImmune = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Chilled] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<NebulaBar>(9).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

        }
    }
}
