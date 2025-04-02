using System;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class PhoenixBlade: CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 106;
            Item.height = 106;
            Item.scale *= 2.2f; //这玩意据说比毁灭剑大两倍 - 真的很大.
            Item.damage = 160;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.rare = ItemRarityID.Purple;
            Item.value = CIShopValue.RarityPricePurple;
            Item.shootSpeed = 12f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //继承至毁灭刀并加强: 对高于70%血量的敌人造成5倍刀刃伤害
            if(target.life >= target.lifeMax * 0.7) 
            {
                hit.Damage *= 4;
                for(int j=0;j<5;j++)
                CIFunction.DustCircle(target.Center, 15, 1.4f, DustID.CrimsonTorch, false, 5f);
                SoundEngine.PlaySound(SoundID.Item74 with {Volume = 0.5f});
            }
            int fuckYou = Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2(0f,0f), ModContent.ProjectileType<FuckYou>(), Item.damage, Item.knockBack, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with {Volume = 0.5f});
            if(fuckYou.WithinBounds(Main.maxProjectiles))
                Main.projectile[fuckYou].DamageType = DamageClass.Melee;
            if(target.life <= target.life * 0.2)
            {
                Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2(0f,0f), 612, Item.damage, Item.knockBack, Main.myPlayer);
                float getSpread = MathHelper.Pi * 0.0174f;
                double startSpread = Math.Atan2(Item.shootSpeed, Item.shootSpeed) - getSpread/2;
                double alterSpread = getSpread/8f;
                double offsetSpread;
                for(int i=0;i<1;i++)
                {
                    float getSpeedX = Main.rand.Next(5);
                    float getSpeedY = Main.rand.Next(3,7);
                    offsetSpread = startSpread + alterSpread * ( i + i * i)/2f + 32f * i;
                    int healProj = Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2((float)(Math.Sin(offsetSpread) * 5f), (float)(Math.Cos(offsetSpread) * 5f)), ModContent.ProjectileType<PhoenixBladeHeal>(), Item.damage, Item.knockBack, Main.myPlayer);
                    int healProjAlter = Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2((float)(Math.Sin(offsetSpread) * 5f), (float)(Math.Cos(offsetSpread) * 5f)), ModContent.ProjectileType<PhoenixBladeHeal>(), Item.damage, Item.knockBack, Main.myPlayer);
                    Main.projectile[healProj].velocity.X = -getSpeedX;
                    Main.projectile[healProj].velocity.Y = -getSpeedY;
                    Main.projectile[healProjAlter].velocity.X = getSpeedX;
                    Main.projectile[healProjAlter].velocity.Y = -getSpeedY;
                }
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if(Main.rand.NextBool(4))   
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.CopperCoin);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BreakerBlade).
                AddIngredient(ItemID.HellstoneBar, 10).
                AddIngredient<EssenceofHavoc>(5).
                AddIngredient(ItemID.SoulofFlight, 3).
                AddIngredient(ItemID.SoulofNight, 3).
                AddIngredient(ItemID.SoulofFright, 3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}