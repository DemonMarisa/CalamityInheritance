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
    public class AncientXerocMask : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 10; //50
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AncientXerocPlateMail>() && legs.type == ModContent.ItemType<AncientXerocCuisses>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = player.Calamity();
            var modPlayer1 = player.CIMod();
            modPlayer.wearingRogueArmor = true;
            modPlayer1.AncientXerocSet = true;
            modPlayer.rogueStealthMax += 1.10f;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            modPlayer.rogueVelocity += 0.10f;

            player.Calamity().WearingPostMLSummonerSet = true;
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
