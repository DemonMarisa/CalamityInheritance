using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientXerocMask : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 16; //50
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
            modPlayer.rogueStealthMax += 1.15f;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if(player.statLife<=(player.statLifeMax2 * 0.8f) && player.statLife > (player.statLifeMax2 * 0.6f))
            {
                player.GetDamage<GenericDamageClass>() +=0.05f;
                player.GetCritChance<GenericDamageClass>() += 5;
            }
            else if(player.statLife<=(player.statLifeMax2 * 0.6f) && player.statLife > (player.statLifeMax2 * 0.4f))
            {
                player.GetDamage<GenericDamageClass>() += 0.15f; //玩家血量40%下的数值加成：25%伤害与25%暴击率
                player.GetCritChance<GenericDamageClass>() += 15;
            }
            else if(player.statLife<=(player.statLifeMax2 * 0.4f) && player.statLife > (player.statLifeMax2 * 0.2f))
            {
                player.GetDamage<GenericDamageClass>() += 0.30f; //玩家血量40%下的数值加成：40%伤害与40%暴击率
                player.GetCritChance<GenericDamageClass>() += 30;
            }
            else if(player.statLife<=(player.statLifeMax2 *0.2f))
            {
                player.GetDamage<GenericDamageClass>() -= 0.30f; //低于20%血量时-30%伤害与暴击率
                player.GetCritChance<GenericDamageClass>() -= 30;
            }
            player.manaCost *= 0.2f;
            modPlayer.rogueVelocity += 0.10f;
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
