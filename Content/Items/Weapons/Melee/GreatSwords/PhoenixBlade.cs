using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;

using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords
{
    public class PhoenixBlade : CIMelee
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 106;
            Item.height = 106;
            Item.scale *= 3.2f; //这玩意据说比毁灭剑大两倍 - 真的很大.
            Item.damage = 160;
            Item.DamageType = GetInstance<TrueMelee>();
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.shootSpeed = 12f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life >= target.lifeMax * 0.7)
            {
                hit.Damage *= 5;
                for (int j = 0; j < 5; j++)
                    CIUtils.DustCircle(target.Center, 15, 1.4f, DustID.CrimsonTorch, false, 5f);
                SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.5f });
            }
            int fuckYou = Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ProjectileID.SolarWhipSwordExplosion, Item.damage, Item.knockBack, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 0.5f });
            Main.projectile[fuckYou].DamageType = DamageClass.Melee;
            float getSpread = MathHelper.Pi * 0.0174f;
            double startSpread = Math.Atan2(Item.shootSpeed, Item.shootSpeed) - getSpread / 2;
            double alterSpread = getSpread / 8f;
            double offsetSpread;
            for (int i = 0; i < 2; i++)
            {
                float getSpeedX = Main.rand.Next(5);
                float getSpeedY = Main.rand.Next(3, 7);
                offsetSpread = startSpread + alterSpread * (i + i * i) / 2f + 32f * i;
                player.SpawnHealProj(target.GetSource_FromThis(), ProjectileType<PhoenixBladeHeal>(), target.Center, new Vector2((float)(Math.Sin(offsetSpread) * 5f), (float)(Math.Cos(offsetSpread) * 5f)));
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.CopperCoin);
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.BreakerBlade).
                    AddIngredient(ItemID.HellstoneBar, 10).
                    AddIngredient(CalamityMaterials.EssenceofHavoc, 6).
                    AddIngredient(ItemID.SoulofFlight, 3).
                    AddIngredient(ItemID.SoulofNight, 3).
                    AddIngredient(ItemID.SoulofFright, 3).
                    AddTile(TileID.Anvils).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.BreakerBlade).
                    AddIngredient(ItemID.HellstoneBar, 10).
                    AddIngredient(ItemID.SoulofFlight, 3).
                    AddIngredient(ItemID.SoulofNight, 3).
                    AddIngredient(ItemID.SoulofFright, 3).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}