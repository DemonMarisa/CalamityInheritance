using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Misc
{
    public class ToxicHeartLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Misc;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.expert = true;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().ToxicHeart = true;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.Plague] = true;
            player.buffImmune[BuffType<CIPlague>()] = true;
        }
        public override void AddRecipes()
        {
        }
        public static void UpdateMiscEffect_ToxicHeart(CIPlayer player)
        {
            if (player.Player.miscCounter % 30 == 0)
            {
                int damage = player.Player.CalcIntDamage<GenericDamageClass>(30);
                foreach (NPC npc in Main.ActiveNPCs)
                {    
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage && Vector2.Distance(player.Player.Center, npc.Center) <= 300f)
                    {
                        if (player.Player.whoAmI == Main.myPlayer)
                        {
                            int dir = npc.Center.X > player.Player.Center.X ? 1 : -1;
                            player.Player.ApplyDamageToNPC(npc, damage, 1f, dir, false, DamageClass.Generic, true);
                        }
                        if (HasCalamity())
                        {
                            if (npc.buffImmune[BuffType<CIPlague>()] && !npc.buffImmune[BuffType<CIPlague>()])
                                return;
                        }
                        else if (npc.buffImmune[BuffType<CIPlague>()])
                            return;
                        if (npc.FindBuffIndex(BuffType<CIPlague>()) == -1)
                            npc.AddBuff(BuffType<CIPlague>(), 120, false);
                    }
                }
            }
        }
    }
}
