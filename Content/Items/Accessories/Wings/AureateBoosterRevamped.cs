using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class AureateBoosterRevamped : CIAccessories
    {
        public override int AccessoriesStyle => Wings;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 8f, 1.5f);
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 54;
            Item.height = 26;
            Item.rare = ItemRarityID.Lime;
            Item.value = CIShopValue.RarityPriceLime;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.CI().ReaverMagic || player.CI().ReaverMelee || player.CI().ReaverRanged|| player.CI().ReaverRogueSet|| player.CI().ReaverSummoner)
            {
                player.pickSpeed -= 0.5f;
                player.fishingSkill += 50;
                player.accLavaFishing = true;
            }
            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                player.rocketDelay2--;
                if (player.rocketDelay2 <= 0)
                {
                    SoundEngine.PlaySound(SoundID.Item13, player.Center);
                    player.rocketDelay2 = 60;
                }
                int dustAmt = 2;
                if (player.controlUp)
                {
                    dustAmt = 4;
                }
                for (int i = 0; i < dustAmt; i++)
                {
                    int type = 6;
                    float scale = 1.75f;
                    int alpha = 100;
                    float x = player.position.X + player.width / 2 + 16f;
                    if (player.direction > 0)
                    {
                        x = player.position.X + player.width / 2 - 26f;
                    }
                    float dustYPos = player.position.Y + player.height - 18f;
                    if (i == 1 || i == 3)
                    {
                        x = player.position.X + player.width / 2 + 8f;
                        if (player.direction > 0)
                        {
                            x = player.position.X + player.width / 2 - 20f;
                        }
                        dustYPos += 6f;
                    }
                    if (i > 1)
                    {
                        dustYPos += player.velocity.Y;
                    }
                    int boosterDust = Dust.NewDust(new Vector2(x, dustYPos), 8, 8, type, 0f, 0f, alpha, default, scale);
                    Dust dust = Main.dust[boosterDust];
                    dust.velocity.X *= 0.1f;
                    dust.velocity.Y = Main.dust[boosterDust].velocity.Y * 1f + 2f * player.gravDir - player.velocity.Y * 0.3f;
                    dust.noGravity = true;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                    if (dustAmt == 4)
                    {
                        dust.velocity.Y += 6f;
                    }
                }
            }
            player.noFallDmg = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.75f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 2.5f;
            constantAscend = 0.125f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
