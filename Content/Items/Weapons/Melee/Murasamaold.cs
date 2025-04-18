﻿using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Weapons.Melee;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Murasamaold : CIMelee, ILocalizedModType
    {
        
        public int frameCounter = 0;
        public int frame = 0;
        public bool IDUnlocked(Player player) => DownedBossSystem.downedYharon || player.name == "Jetstream Sam" || player.name == "Samuel Rodrigues";

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 13));
            //干掉魂设定，我要转微光啊
            ItemID.Sets.AnimatesAsSoul[Type] = false;
            if(CIServerConfig.Instance.CustomShimmer == true) //关闭微光转化后，利维坦龙涎香正常掉落
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Murasamaold>()] = ModContent.ItemType<Murasama>();
                //开启微光转化时，Murasama将会以如下顺序进行微光转化
                //灾厄版本的鬼妖村正->使用新挥刀特效的大范围鬼妖村正->1457的传统鬼妖村正
            }
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 128;
            Item.width = 56;
            Item.damage = 20001;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.knockBack = 6.5f;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ModContent.ProjectileType<MurasamaSlashold>();
            Item.shootSpeed = 24f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
        }

        public override bool MeleePrefix() => true;
        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture;

            if (IDUnlocked(Main.LocalPlayer))
            {
                //0 = 6 frames, 8 = 3 frames]
                texture = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }
            else
            {
                texture = ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture;

            if (IDUnlocked(Main.LocalPlayer))
            {
                texture = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            else
            {
                texture = ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, null, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (!IDUnlocked(Main.LocalPlayer))
                return;
            Texture2D texture = ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/MurasamaGlowold").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, frame == 0 ? 36 : frame == 8 ? 24 : 6, 13, false), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] > 0)
                return false;
            return IDUnlocked(player);
        }
        public override void AddRecipes()
        {
            if(CIServerConfig.Instance.CustomShimmer == false) 
            //关闭微光转化则采用手动的方式
            {
                CreateRecipe().
                    AddIngredient<MurasamaNeweffect>().
                    Register();
            }
        }       
    }
}
