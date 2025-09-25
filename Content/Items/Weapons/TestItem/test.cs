using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Rogue;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.TestItem
{
    public class Test : CIMelee, ILocalizedModType
    {
        //别改这个为大写了，他每次拉去的时候图片的文件总是变成小写 
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Generic;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 0;
            Item.shoot = ModContent.ProjectileType<ScarletDevilBullet>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // float starSpeed = 25f;
            /*
            // Spawn a circle of fast bullets.
            for (int i = 0; i < 40; i++)
            {
                Vector2 shootVelocity = (MathHelper.TwoPi * i / 40f).ToRotationVector2() * starSpeed;
                int bullet = Projectile.NewProjectile(source, player.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<ScarletDevilBullet>(), (int)(damage * 0.01), 0f);
                if (Main.projectile.IndexInRange(bullet))
                    Main.projectile[bullet].Calamity().stealthStrike = true;
            }
            */
            // Spawn a pair of stars, one slow, one fast.
            /*
            int pointsOnStar = 6;
            for (int k = 0; k < CIConfig.Instance.Debugint; k++)
            {
                for (int i = 0; i < CIConfig.Instance.Debugint2; i++)
                {
                    // 基础角度：MathHelper.Pi * 1.5f 对应 270°（Terraria坐标系中正上方） 居然是这样
                    // 顶点分布计算
                    // 将圆六等分
                    // i = 0: 270° (正上)
                    // i = 1: 210° (左下)
                    // i = 2: 150° (右下)
                    // i = 3: 90°  (正下)
                    // i = 4: 30°  (右上)
                    // i = 5: 330° (右下)
                    float angle = i * MathHelper.TwoPi / pointsOnStar - MathHelper.Pi * 1.5f;
                    // 生成三条对角线
                    // 0→3(正上→正下)
                    // 1→4(左下→右上)
                    // 2→5(右下→左上)
                    float nextAngle = (i + 3) * MathHelper.TwoPi / pointsOnStar - MathHelper.Pi * 1.5f;

                    if (k == 1)
                        // 连接间隔2个的顶点（i → i+2），形成两个三角形
                        nextAngle = (i + 2) * MathHelper.TwoPi / pointsOnStar - MathHelper.Pi * 1.5f;

                    Vector2 start = angle.ToRotationVector2();
                    Vector2 end = nextAngle.ToRotationVector2();
                    int pointsOnStarSegment = 18;
                    for (int j = 0; j < pointsOnStarSegment; j++)
                    {
                        Vector2 shootVelocity = Vector2.Lerp(start, end, j / (float)pointsOnStarSegment) * starSpeed;
                        int bullet = Projectile.NewProjectile(source, player.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<ScarletDevilBullet>(), (int)(damage * 0.01), 0f);
                        if (Main.projectile.IndexInRange(bullet))
                            Main.projectile[bullet].Calamity().stealthStrike = true;
                    }
                }
            }
            */
            /*
            float anglestep = MathHelper.TwoPi / pointsOnStar;
            float angle = 0 * anglestep - MathHelper.Pi * 1.5f;
            // 生成三条对角线
            // 0→3(正上→正下)
            // 1→4(左下→右上)
            // 2→5(右下→左上)
            float nextAngle = (0 + 3) * anglestep - MathHelper.Pi * 1.5f;

            Vector2 start = angle.ToRotationVector2();
            Vector2 end = nextAngle.ToRotationVector2();
            int pointsOnStarSegment = 18;
            for (int j = 0; j < pointsOnStarSegment; j++)
            {
                Vector2 shootVelocity = Vector2.Lerp(start, end, j / (float)pointsOnStarSegment) * starSpeed;
                int bullet = Projectile.NewProjectile(source, player.Center + shootVelocity, shootVelocity, ModContent.ProjectileType<ScarletDevilBullet>(), (int)(damage * 0.01), 0f);
                if (Main.projectile.IndexInRange(bullet))
                    Main.projectile[bullet].Calamity().stealthStrike = true;
            }
            */
            
            return false;
        }
        public override bool? UseItem(Player player)
        {
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            /*
            if (player.altFunctionUse == 2)
                Main.NewText($"Calamitas Clone P2: {CIGlobalNPC.LegacyCalamitasCloneP2}");
            else
                Main.NewText($"Calamitas Clone P1: {CIGlobalNPC.LegacyCalamitasClone}");
            */
            /*
            if (player.altFunctionUse == 2)
                Main.NewText($"LegacyYharon P2: {CIGlobalNPC.LegacyYharon}");
            else
                Main.NewText($"Calamitas Clone P1: {CIGlobalNPC.LegacyCalamitasClone}");
            */
            /*
            Main.NewText($"PlayerLife: {player.lifeRegen}");
            Main.NewText($"PlayerSR: {player.CIMod().AncientSilvaRegenCounter}");
            */
            /*
            if (player.altFunctionUse == 2 && MusicChoiceUI.ChangeCd == 0)
                MusicChoiceUI.active = !MusicChoiceUI.active;
            else
            {
                Main.NewText($"MusicChoiceUIProg: {MusicChoiceUI.aniProg}");
                Main.NewText($"ArrowBehavior: {ArrowBehavior.FadeTime}");
                Main.NewText($"MusicBoxVerBehavior: {MusicBoxVerBehavior.FadeTime}");
                Main.NewText($"NorVerBehavior: {NorVerBehavior.FadeTime}");
                Main.NewText($"PianoVerBehavior: {PianoVerBehavior.FadeTime}");
            }
            */
            return true;
        }
    }
}
