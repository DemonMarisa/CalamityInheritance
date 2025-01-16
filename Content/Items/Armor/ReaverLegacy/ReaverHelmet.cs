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
using SteelSeries.GameSense;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelmet : ModItem, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
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
            return body.type == ModContent.ItemType<ReaverScaleMail>() && legs.type == ModContent.ItemType<ReaverCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }
        
        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            //我不知道咋写仆从，先留在这里后面在考虑改了
            //2025,1,12在改了
            modPlayer1.reaverSummoner = true;
            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = player.ApplyArmorAccDamageBonusesTo(80);
                var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                var source = player.GetSource_ItemUse(Item);
                if (player.FindBuffIndex(ModContent.BuffType<ReaverSummonSetBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<ReaverSummonSetBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<ReaverOrbOld>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<ReaverOrbOld>(), damage, 2f, player.whoAmI);
                }
            }
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 1.2f;
            player.whipRangeMultiplier += 1.2f;
            //Scarlet:将伤害加成全部转移至头盔，鞭子加成转移至套装奖励（因为原版就这么做的）
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.moveSpeed += 0.1f;
            player.GetDamage<SummonDamageClass>() += 0.25f; //总40%伤害加成
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<PerennialBar>(),5)
            .AddIngredient(ItemID.JungleSpores, 4)
            .AddIngredient(ModContent.ItemType<EssenceofEleum>(), 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
