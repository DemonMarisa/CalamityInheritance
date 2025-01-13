using CalamityInheritance;
using CalamityInheritance.Utilities;
using CalamityInheritance.Buffs;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Rarities;
using Mono.Cecil;
using CalamityInheritance.Buffs.Summon;
using CalamityMod.Buffs.Pets;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.ArmorProj;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Reaver Helmet");
            /* Tooltip.SetDefault("5% increased minion damage, +2 max minions, and increased minion knockback\n" +
                "10% increased movement speed and can move freely through liquids"); */

        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ModContent.RarityType<PureGreen>();
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
            player.GetDamage<SummonDamageClass>() += 0.24f; // 40%召唤伤害，无栏位
            player.whipRangeMultiplier += 1.2f;
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.ignoreWater = true;
            player.GetDamage<SummonDamageClass>() += 0.01f;
            player.GetCritChance<RangedDamageClass>() += 1;
            player.GetAttackSpeed<MeleeDamageClass>() +=0.01f;
            player.manaCost *=0.99f;
            modPlayer.rogueStealthMax += 0.01f;
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
