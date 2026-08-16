using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Misc
{
    public class FungalCarapace : CIAccessories
    {
        public override int AccessoriesStyle => Misc;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.defense = 6;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().FungalCarapaceHurt = true;
        }
        public static void Hurt(Player.HurtInfo info, Player player)
        {
            var source = player.GetSource_Accessory(ItemLoader.GetItem(ItemType<FungalCarapace>()).Item);
            if (info.Damage > 0)
            {
                // 播放声音
                SoundEngine.PlaySound(SoundID.NPCHit45, player.position);

                // 弹幕生成参数
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(player.velocity.X, player.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                int fDamage = (int)player.CalcIntDamage<GenericDamageClass>(70);

                if (player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        // 确定生成位置
                        float xPos = Main.rand.NextBool() ? player.Center.X + 100 : player.Center.X - 100;
                        Vector2 spawnPos = new Vector2(xPos, player.Center.Y + Main.rand.Next(-100, 101));

                        // 计算角度
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                        // 创建弹幕
                        var spore1 = Projectile.NewProjectileDirect(
                            source,
                            spawnPos,
                            new Vector2((float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f)),
                            ProjectileID.TruffleSpore,
                            fDamage,
                            1.25f,
                            player.whoAmI
                        );

                        var spore2 = Projectile.NewProjectileDirect(
                            source,
                            spawnPos,
                            new Vector2((float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f)),
                            ProjectileID.TruffleSpore,
                            fDamage,
                            1.25f,
                            player.whoAmI
                        );

                        // 设置时间
                        spore1.timeLeft = 300;
                        spore2.timeLeft = 300;
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 15);
            recipe.Register();
        }
    }
}
