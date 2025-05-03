// using Terraria.ID;
// using Terraria;
// using Terraria.ModLoader;
// using CalamityInheritance.Content.Items.Weapons;
// using CalamityInheritance.Utilities;
// using CalamityInheritance.CIPlayer;
// using System.Security.Authentication;

// namespace CalamityInheritance.Content.Items
// {
//     public class Test : CIMelee, ILocalizedModType
//     {
//         public static string WeaponRoute => "CalamityInheritance/Content/Items";
//         //别改这个为大写了，他每次拉去的时候图片的文件总是变成小写 
//         public override string Texture => $"{WeaponRoute}/Test";
//         public override void SetStaticDefaults()
//         {
//             ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
//         }
//         public override void SetDefaults()
//         {
//             Item.width = 42;
//             Item.damage = 55;
//             Item.DamageType = DamageClass.Generic;
//             Item.useAnimation = 15;
//             Item.useTime = 15;
//             Item.useTurn = true;
//             Item.useStyle = ItemUseStyleID.Swing;
//             Item.knockBack = 5f;
//             Item.UseSound = CISoundID.SoundWeaponSwing;
//             Item.autoReuse = true;
//             Item.height = 42;
//             Item.rare = ItemRarityID.Orange;
//             Item.shootSpeed = 10;
//             Item.shoot = ModContent.ProjectileType<AlphaBeam>();
//         }/*
//         public override bool CanUseItem(Player player)
//         {
//             return true;
//         }*/
//         public override bool AltFunctionUse(Player player)
//         {
//             return true;
//         }
//         /*
//         public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
//         {
//             int fireOffset = -100;
//             Vector2 mousePos = Main.MouseWorld;
//             int totalFire = 4;
//             int firePosX = (int)(mousePos.X + player.Center.X) / 2;
//             int firePosY = (int)player.Center.Y;

//             for (int fireCount = 0; fireCount < totalFire; fireCount++)
//             {
//                 // 垂直偏移计算
//                 Vector2 finalPos = new Vector2(firePosX, firePosY + fireOffset * fireCount);

//                 // 计算朝向鼠标的方向
//                 Vector2 direction = mousePos - finalPos;
//                 direction.Normalize();

//                 // 随机30度发射
//                 direction = direction.RotatedByRandom(MathHelper.ToRadians(15));

//                 // 保持原速度并应用新方向
//                 Vector2 newVelocity = direction * velocity.Length();

//                 int projectileFire = Projectile.NewProjectile(source, finalPos, newVelocity, ModContent.ProjectileType<Galaxia2>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
//                 Main.projectile[projectileFire].timeLeft = 160;
//             }
            
//             return false;
//         }*/
//         public override bool AltFunctionUse(Player player) => true;
//         public override bool? UseItem(Player player)
//         {
//             CalamityInheritancePlayer cIPlayer = player.CIMod();
//             cIPlayer.meleeLevel = 0;
//             cIPlayer.meleePool = 0;
//             cIPlayer.rangeLevel = 0;
//             cIPlayer.rangePool = 0;
//             cIPlayer.magicLevel = 0;
//             cIPlayer.magicPool = 0;
//             cIPlayer.summonLevel = 0;
//             cIPlayer.summonPool = 0;
//             cIPlayer.rogueLevel = 0;
//             cIPlayer.roguePool = 0;
//             cIPlayer.DukeTier1 = false;
//             cIPlayer.DukeTier2 = false;
//             cIPlayer.DukeTier3 = false;
//             cIPlayer.BetsyTier1= false;
//             cIPlayer.BetsyTier2= false;
//             cIPlayer.BetsyTier3= false;
//             cIPlayer.DefendTier1 = false;
//             cIPlayer.DefendTier2 = false;
//             cIPlayer.DefendTier3 = false;
//             cIPlayer.PlanteraTier1 = false;
//             cIPlayer.PlanteraTier2 = false;
//             cIPlayer.PlanteraTier3 = false;
//             cIPlayer.DestroyerTier1 = false;
//             cIPlayer.DestroyerTier2 = false;
//             cIPlayer.DestroyerTier3 = false;
//             cIPlayer.ColdDivityTier1 = false;
//             cIPlayer.ColdDivityTier2 = false;
//             cIPlayer.ColdDivityTier3 = false;
//             cIPlayer.PBGTier1 = false;
//             cIPlayer.PBGTier2 = false;
//             cIPlayer.PBGTier3 = false;
//             if (player.altFunctionUse == 2)
//             {
//                 cIPlayer.DukeTier1 = true;
//                 cIPlayer.DukeTier2 = true;
//                 cIPlayer.DukeTier3 = true;
//                 cIPlayer.BetsyTier1 = true;
//                 cIPlayer.BetsyTier2 = true;
//                 cIPlayer.BetsyTier3 = true;
//                 cIPlayer.DefendTier1 = true;
//                 cIPlayer.DefendTier2 = true;
//                 cIPlayer.DefendTier3 = true;
//                 cIPlayer.PlanteraTier1 = true;
//                 cIPlayer.PlanteraTier2 = true;
//                 cIPlayer.PlanteraTier3 = true;
//                 cIPlayer.DestroyerTier1 = true;
//                 cIPlayer.DestroyerTier2 = true;
//                 cIPlayer.DestroyerTier3 = true;
//                 cIPlayer.ColdDivityTier1 = true;
//                 cIPlayer.ColdDivityTier2 = true;
//                 cIPlayer.ColdDivityTier3 = true;
//                 cIPlayer.PBGTier1 = true;
//                 cIPlayer.PBGTier2 = true;
//                 cIPlayer.PBGTier3 = true;
//             }
//         }
//     }
// }
