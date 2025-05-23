﻿using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Wulfrum
{
    public class WulfrumStaff : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<WulfrumProsthesis>(false);
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 2;
            Item.width = 44;
            Item.height = 46;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<WulfrumStaffHoldOut>();
            Item.shootSpeed = 9f;

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.damage = 1145;
                Item.rare = ModContent.RarityType<IchikaBlack>();
                Item.value = CIShopValue.RarityPricePureRed;
                Item.UseSound = CISoundID.SoundFart;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else
            {
                Item.damage = 13;
                Item.rare = ItemRarityID.Blue;
                Item.value = CIShopValue.RarityPriceBlue;
                Item.UseSound = CISoundID.SoundStaffDiamond;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<WulfrumStaffHoldOut>()] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo projSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.ownedProjectileCounts[ModContent.ProjectileType<WulfrumStaffHoldOut>()] < 1)
                Projectile.NewProjectileDirect(projSource, position, velocity, ModContent.ProjectileType<WulfrumStaffHoldOut>(), damage, knockback, player.whoAmI);

            if (!Main.zenithWorld)
                return false;

            int[] pType=
            [
                ModContent.ProjectileType<GalaxyStarold>(),
                ModContent.ProjectileType<ProfanedNuke>(),
                ModContent.ProjectileType<ExoFlareClusterold>(),
                ModContent.ProjectileType<ChickenRound>(),
                ModContent.ProjectileType<Celestus2>(),
                ModContent.ProjectileType<DragonRageProj>(),
                ModContent.ProjectileType<PhantasmalRuinProjold>(),
                ModContent.ProjectileType<RogueTypeHammerGalaxySmasherProjClone>(),
                ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>(),
                ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>(),
                ModContent.ProjectileType<RogueTypeHammerTruePaladinsProjClone>(),
                ModContent.ProjectileType<SupernovaBombold>(),
            ];
            if (!Main.zenithWorld)
            {
                Projectile.NewProjectile(projSource, position, velocity, type, damage, knockback, player.whoAmI, 0, 0, 1f);
                Projectile.NewProjectile(projSource, position, velocity, type, damage, knockback, player.whoAmI, 0, 0, -1f);
            }
            else
            {
                float rotAngle = 360f / pType.Length;
                for (int j = 0; j < pType.Length; j++)
                {
                    //随机get数组内的一个元素, 方便给予随机性
                    int whatType = j + Main.rand.Next(1,4);
                    //如果发现其比数组长度大
                    if(whatType >= pType.Length)
                    {
                        //将其重新赋值
                        whatType = pType.Length - Main.rand.Next(1,5);
                    }
                    //获取这个元素
                    int getType = pType[whatType];
                    //开始处理发射逻辑
                    float rot = MathHelper.ToRadians(j * rotAngle);
                    Vector2 spreadPos = new Vector2(velocity.X, 0f).RotatedBy(rot);
                    Vector2 spreadVel = new Vector2(velocity.X, 0f).RotatedBy(rot); 
                    int i = Projectile.NewProjectile(projSource, spreadPos, spreadVel* 2, getType, damage, knockback, player.whoAmI);
                    Main.projectile[i].DamageType = DamageClass.Magic;
                    Main.projectile[i].ArmorPenetration += 100;
                    Main.projectile[i].usesLocalNPCImmunity = true;
                    Main.projectile[i].localNPCHitCooldown = 10;
                }
            }
            
            Vector2 targetPosition = Main.MouseWorld;
            player.itemRotation = CIFunction.CalculateItemRotation(player, targetPosition, -18);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap> (12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
