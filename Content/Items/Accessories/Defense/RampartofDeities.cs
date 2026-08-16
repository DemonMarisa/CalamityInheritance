using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Restorative;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    [AutoloadEquip(EquipType.Shield)]
    public class RampartofDeities : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 64;
            Item.height = 62;
            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.pStone = true;
            player.LAP().ExImmuneTime += 40;
            player.lifeRegen += 4;
            player.GetArmorPenetration<GenericDamageClass>() += 50;
            player.CI().RampartOfDeitiesStar = true;

            player.noKnockback = true;
            if (player.statLife > player.statLifeMax2 * 0.5)
                player.GetDamage<GenericDamageClass>() += 0.1f;
            if (player.statLife <= player.statLifeMax2 * 0.5)
            {
                player.statDefense += 20;
                player.AddBuff(BuffID.IceBarrier, 5);
            }
            if (player.statLife <= player.statLifeMax2 * 0.15)
                player.AddDR(0.5f);

            if (player.statLife >= player.statLifeMax2 * 0.25f)
            {
                player.hasPaladinShield = true;
                if (player.whoAmI != Main.myPlayer && player.miscCounter % 10 == 0)
                {
                    int myPlayer = Main.myPlayer;
                    if (Main.player[myPlayer].team == player.team && player.team != 0)
                    {
                        float teamPlayerXDist = player.position.X - Main.player[myPlayer].position.X;
                        float teamPlayerYDist = player.position.Y - Main.player[myPlayer].position.Y;
                        if ((float)Math.Sqrt(teamPlayerXDist * teamPlayerXDist + teamPlayerYDist * teamPlayerYDist) < 800f)
                            Main.player[myPlayer].AddBuff(BuffID.PaladinsShield, 20);
                    }
                }
            }
        }
        public static void RampartOfDeitiesStar_OnHurt(CIPlayer player)
        {
            var source = player.Player.GetSource_Accessory(ItemLoader.GetItem(ItemType<DeificAmuletLegacy>()).Item);
            for (int n = 0; n < 5; n++)
            {
                int deificStarDamage = (int)player.Player.GetTotalDamage<GenericDamageClass>().ApplyTo(3000);

                Projectile star = CIUtils.ProjectileRain(source, player.Player.Center, 400f, 100f, 500f, 800f, 29f,
                ProjectileID.StarVeilStar, deificStarDamage, 4f, player.Player.whoAmI);
                star.DamageType = DamageClass.Generic;
                star.usesLocalNPCImmunity = true;
                star.localNPCHitCooldown = 5;
            }
        }
        public override void AddRecipes()
        {
            //CreateRecipe().
            //    AddIngredient(ItemType<FrigidBulwark>()).
            //    AddIngredient<CosmiliteBar>(10).
            //    AddIngredient<DeificAmuletLegacy>().
            //    AddIngredient<AscendantSpiritEssence>(4).
            //    AddTile<CosmicAnvil>().
            //    Register();
        }
    }
}
