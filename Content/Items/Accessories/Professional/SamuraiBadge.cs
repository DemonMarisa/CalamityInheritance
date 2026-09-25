using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class SamuraiBadge : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        internal const float MaxBonus = 0.3f;
        internal const float MaxDistance = 600f;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = ItemRarityID.Purple;
            Item.value = CIShopValue.RarityPricePurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            float bonus = CalculateBonus(player);
            player.GetAttackSpeed<MeleeDamageClass>() += bonus;
            player.GetDamage<MeleeDamageClass>() += bonus;
            player.BoostTrueMelee(bonus);
        }

        private static float CalculateBonus(Player player)
        {
            float bonus = 0f;

            int closestNPC = -1;
            foreach (NPC nPC in Main.ActiveNPCs)
            {
                if (!nPC.friendly && !nPC.dontTakeDamage)
                {
                    closestNPC = nPC.whoAmI;
                    break;
                }
            }
            float distance = -1f;
            foreach (NPC nPC in Main.ActiveNPCs)
            {
                if (!nPC.friendly && !nPC.dontTakeDamage)
                {
                    float distance2 = Math.Abs(nPC.position.X + nPC.width / 2 - (player.position.X + player.width / 2)) + Math.Abs(nPC.position.Y + nPC.height / 2 - (player.position.Y + player.height / 2));
                    if (distance == -1f || distance2 < distance)
                    {
                        distance = distance2;
                        closestNPC = nPC.whoAmI;
                    }
                }
            }

            if (closestNPC != -1)
            {
                NPC actualClosestNPC = Main.npc[closestNPC];

                float generousHitboxWidth = Math.Max(actualClosestNPC.Hitbox.Width / 2f, actualClosestNPC.Hitbox.Height / 2f);
                float hitboxEdgeDist = actualClosestNPC.Distance(player.Center) - generousHitboxWidth;

                if (hitboxEdgeDist < 0)
                    hitboxEdgeDist = 0;

                if (hitboxEdgeDist < MaxDistance)
                {
                    bonus = MathHelper.Lerp(0f, MaxBonus, 1f - hitboxEdgeDist / MaxDistance);

                    if (bonus > MaxBonus)
                        bonus = MaxBonus;
                }
            }

            return bonus;
        }
        public override void AddRecipes()
        {
        }
    }
}
