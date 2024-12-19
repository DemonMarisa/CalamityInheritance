using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class HeavenlyGaleold : ModItem
    {
            public const float NormalArrowDamageMult = 1.25f;
            private static int[] ExoArrows;
            public override void SetStaticDefaults()
            {
                ExoArrows = new int[]
                {
                ModContent.ProjectileType<ExoArrowTeal>(),
                ModContent.ProjectileType<OrangeExoArrow>(),
                ModContent.ProjectileType<ExoArrowGreen>(),
                ModContent.ProjectileType<ExoArrowBlue>()
                };
                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }

            public override void SetDefaults()
            {
                Item.damage = 198;
                Item.DamageType = DamageClass.Ranged;
                Item.width = 44;
                Item.height = 58;
                Item.useTime = 15;
                Item.useAnimation = 30;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noMelee = true;
                Item.knockBack = 4f;
                Item.UseSound = SoundID.Item5;
                Item.autoReuse = true;
                Item.shoot = ProjectileID.WoodenArrowFriendly;
                Item.shootSpeed = 12f;
                Item.useAmmo = AmmoID.Arrow;
                Item.rare = ModContent.RarityType<Violet>();
                Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
                Item.Calamity().canFirePointBlankShots = true;
            }

            public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
            {
                // 获取玩家的射击起始位置
                Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
                Vector2 baseOffset = velocity;
                baseOffset.Normalize();  // 方向归一化
                baseOffset *= 40f;       // 基础偏移量，用于放置箭矢起始位置

                float piOver10 = MathHelper.Pi / 10f;  // π / 10 的常量值
                bool againstWall = !Collision.CanHit(source, 0, 0, source + baseOffset, 0, 0);

                int numArrows = 5;  // 箭矢数量
                float dmgMult = 1f; // 初始伤害倍率

                for (int i = 0; i < numArrows; i++)
                {
                    // 计算每个箭矢的偏移位置
                    float offsetAmt = i - (numArrows - 1f) / 2f;
                    Vector2 offset = baseOffset.RotatedBy(piOver10 * offsetAmt);

                    // 如果目标位置无法直接命中，则将箭矢向后调整
                    if (againstWall)
                        offset -= baseOffset;

                    int thisArrowType = type;  // 默认箭矢类型

                    // 检查箭矢类型是否为普通木箭
                    if (type == ProjectileID.WoodenArrowFriendly)
                    {
                        // 随机选择 ExoArrows 类型
                        thisArrowType = Main.rand.Next(ExoArrows);

                        // 对特殊箭矢（TealExoArrow）应用伤害倍率
                        dmgMult = thisArrowType == ModContent.ProjectileType<ExoArrowTeal>() ? 0.66f : 1f;
                    }
                    else
                    {
                        dmgMult = NormalArrowDamageMult;
                    }

                    int finalDamage = (int)(damage * dmgMult);
                    int proj = Projectile.NewProjectile(spawnSource, source + offset, velocity, thisArrowType, finalDamage, knockback, player.whoAmI);

                    if (type != ProjectileID.WoodenArrowFriendly)
                    {
                        Main.projectile[proj].noDropItem = true;
                    }
                }

                return false;
            }

            public override bool CanConsumeAmmo(Item ammo, Player player)
            {
                if (Main.rand.Next(0, 100) < 66)
                    return false;
                return true;
            }

    }
}
