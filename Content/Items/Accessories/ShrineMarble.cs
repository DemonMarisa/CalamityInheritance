using System.Numerics;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
namespace CalamityInheritance.Content.Items.Accessories
{
    public class ShrineMarble : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 36;
            Item.width = 42;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.CIMod();
            p.SMarble = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<SMarbleSwordBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<SMarbleSwordBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<ShrineMarbleSword>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(),player.Center, Vector2.Zero, ModContent.ProjectileType<ShrineMarbleSword>(), (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(20), 6f, Main.myPlayer);
                    Projectile.NewProjectile(player.GetSource_FromThis(),player.Center, Vector2.Zero, ModContent.ProjectileType<ShrineMarbleSwordClone>(), (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(20), 6f, Main.myPlayer);
                }
            }
        }
    }
}