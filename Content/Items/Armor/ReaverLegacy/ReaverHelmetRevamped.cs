using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Buffs.Summon;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.ArmorProj;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelmetRevamped : CIArmor, ILocalizedModType 
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 7; //40
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<ReaverScaleMailRevamped>() && legs.type == ItemType<ReaverCuissesRevamped>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }
        
        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CIMod();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            //我不知道咋写仆从，先留在这里后面在考虑改了
            //2025,1,12在改了
            modPlayer1.ReaverSummoner = true;
            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 80;
                var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                var source = player.GetSource_ItemUse(Item);
                if (player.FindBuffIndex(BuffType<ReaverSummonSetBuff>()) == -1)
                {
                    player.AddBuff(BuffType<ReaverSummonSetBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ProjectileType<ReaverOrbOld>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, Vector2.Zero, ProjectileType<ReaverOrbOld>(), damage, 2f, player.whoAmI);
                }
            }
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 0.6f;
            player.whipRangeMultiplier += 0.5f;
            //Scarlet:平摊数值加成，鞭 速度110%，鞭 距离100%，20%召唤伤害
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 0.5f;
            player.whipRangeMultiplier += 0.5f;
            player.GetDamage<SummonDamageClass>() += 0.05f; //
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<PerennialBar>(),5)
            .AddIngredient(ItemID.JungleSpores, 4)
            .AddIngredient(ItemType<EssenceofEleum>(), 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
