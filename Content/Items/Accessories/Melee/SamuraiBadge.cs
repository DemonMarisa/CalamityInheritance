using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Items.LoreItems;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class SamuraiBadge : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        internal const float MaxBonus = 0.3f;
        internal const float MaxDistance = 600f;
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:30,
            itemHeight:30,
            itemRare:ItemRarityID.Purple,
            itemValue:CIShopValue.RarityPricePurple
        );
        public override void ExSSD()
        {
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            float bonus = CalculateBonus(player);
            player.GetAttackSpeed<MeleeDamageClass>() += bonus;
            player.GetDamage<MeleeDamageClass>() += bonus;
            player.GetDamage<TrueMeleeDamageClass>() += bonus;
        }

        private static float CalculateBonus(Player player)
        {
            float bonus = 0f;

            int closestNPC = -1;
            foreach (NPC nPC in Main.ActiveNPCs)
            {
                if (nPC.IsAnEnemy() && !nPC.dontTakeDamage)
                {
                    closestNPC = nPC.whoAmI;
                    break;
                }
            }
            float distance = -1f;
            foreach (NPC nPC in Main.ActiveNPCs)
            {
                if (nPC.IsAnEnemy() && !nPC.dontTakeDamage)
                {
                    float distance2 = Math.Abs(nPC.position.X + (float)(nPC.width / 2) - (player.position.X + (float)(player.width / 2))) + Math.Abs(nPC.position.Y + (float)(nPC.height / 2) - (player.position.Y + (float)(player.height / 2)));
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
                    bonus = MathHelper.Lerp(0f, MaxBonus, 1f - (hitboxEdgeDist / MaxDistance));

                    if (bonus > MaxBonus)
                        bonus = MaxBonus;
                }
            }

            return bonus;
        }
        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe().
                    AddIngredient<KnowledgeProvidence>().
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
