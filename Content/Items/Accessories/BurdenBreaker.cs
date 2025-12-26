using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Projectiles.Typeless;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class BurdenBreaker : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.master = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
            { return; }
            // Completely remove movement restrictions if you're yeeting with the profaned spear
            if (player.ownedProjectileCounts[ModContent.ProjectileType<RelicOfDeliveranceSpear>()] <= 0)
            {
                if (player.velocity.X > 5f)
                {
                    player.velocity.X *= 1.025f;
                    if (player.velocity.X >= 500f)
                    {
                        player.velocity.X = 0f;
                    }
                }
                else if (player.velocity.X < -5f)
                {
                    player.velocity.X *= 1.025f;
                    if (player.velocity.X <= -500f)
                    {
                        player.velocity.X = 0f;
                    }
                }
            }
        }
    }
}
