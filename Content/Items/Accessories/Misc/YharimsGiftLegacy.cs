using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Typeless.Explosions;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Misc
{
    public class YharimsGiftLegacy : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Misc;
        public int dragonTimer = 60;
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 70;
            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var source = player.GetSource_Accessory(Item);
            player.moveSpeed += 0.15f;
            player.GetDamage<GenericDamageClass>() += 0.15f;
            if (player.velocity.Length() > 0.05f)
            {
                dragonTimer--;
                if (dragonTimer <= 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage = player.CalcIntDamage<GenericDamageClass>(750);
                        int projectile1 = Projectile.NewProjectile(source, player.Center, Vector2.Zero, ProjectileType<DragonDust>(), damage, 5f, player.whoAmI, 0f, 0f);
                        Main.projectile[projectile1].timeLeft = 60;
                    }
                    dragonTimer = 60;
                }
            }
            else
            {
                dragonTimer = 60;
            }
            if (player.immune)
            {
                if (player.miscCounter % 8 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int damage= player.CalcIntDamage<GenericDamageClass>(1500);
                        CIUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileType<SkyFlareFriendly>(), damage, 9f, player.whoAmI);
                    }
                }
            }
        }
    }
}
