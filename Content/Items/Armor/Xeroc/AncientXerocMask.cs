using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Armor.Empyrean;
using CalamityMod.CalPlayer;
using CalamityInheritance.Buffs.Statbuffs;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientXerocMask : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<EmpyreanMask>(false);
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
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
            var modPlayer = player.Calamity();
            modPlayer.wearingRogueArmor = true;
            modPlayer.rogueStealthMax += 1.10f;
            modPlayer.rogueVelocity += 0.10f;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            XerocSetbouns(player, modPlayer);
            player.Calamity().WearingPostMLSummonerSet = true;
        }
        public static void XerocSetbouns(Player player, CalamityPlayer calPlayer)
        {
            calPlayer.stealthStrikeHalfCost = true;
            if (player.statLife <= (player.statLifeMax2 * 0.8f) && player.statLife > (player.statLifeMax2 * 0.6f))
            {
                player.GetDamage<GenericDamageClass>() += 0.10f;
                player.GetCritChance<GenericDamageClass>() += 10;
            }

            else if (player.statLife <= (player.statLifeMax2 * 0.6f) && player.statLife > (player.statLifeMax2 * 0.25f))
            {
                player.GetDamage<GenericDamageClass>() += 0.15f;
                player.GetCritChance<GenericDamageClass>() += 15;
            }

            else if (player.statLife <= (player.statLifeMax2 * 0.25f) && player.statLife > (player.statLifeMax2 * 0.15f))
            {
                player.AddBuff(BuffType<AncientXerocMadness>(), 2);
                player.GetDamage<GenericDamageClass>() += 0.40f;
                player.GetCritChance<GenericDamageClass>() += 40;
                player.manaCost *= 0.10f;
                calPlayer.healingPotionMultiplier += 0.10f;
            }
            else if (player.statLife <= (player.statLifeMax2 * 0.15f))
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
