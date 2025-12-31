using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{ 
    public class HellkiteLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public static float PitchSound = 1;
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 84;
            Item.damage = 180;
            Item.DamageType = GetInstance<TrueMeleeDamageClass>();
            Item.useTime = Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (!Main.zenithWorld)
                if (Main.rand.NextBool(4))
                    Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.InfernoFork);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 300);
            int onHitDamage = player.CalcIntDamage<MeleeDamageClass>(Item.damage);
            player.ApplyDamageToNPC(target, onHitDamage, 0f, 0, false);
            float firstDustScale = 1.7f;
            float secondDustScale = 0.8f;
            float thirdDustScale = 2f;
            Vector2 dustRotation = (target.rotation - 1.57079637f).ToRotationVector2();
            Vector2 dustVelocity = dustRotation * target.velocity.Length();
            SoundEngine.PlaySound(SoundID.Item14, target.Center);
            int increment;
            for (int i = 0; i < 40; i = increment + 1)
            {
                int swingDust = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.InfernoFork, 0f, 0f, 200, default, firstDustScale);
                Dust dust = Main.dust[swingDust];
                dust.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)target.width / 2f;
                dust.noGravity = true;
                dust.velocity.Y -= 6f;
                dust.velocity *= 3f;
                dust.velocity += dustVelocity * Main.rand.NextFloat();
                swingDust = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.InfernoFork, 0f, 0f, 100, default, secondDustScale);
                dust.position = target.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)target.width / 2f;
                dust.velocity.Y -= 6f;
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.color = Color.Crimson * 0.5f;
                dust.velocity += dustVelocity * Main.rand.NextFloat();
                increment = i;
            }
            for (int j = 0; j < 20; j = increment + 1)
            {
                int swingDust2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.InfernoFork, 0f, 0f, 0, default, thirdDustScale);
                Dust dust = Main.dust[swingDust2];
                dust.position = target.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)target.velocity.ToRotation(), default) * (float)target.width / 3f;
                dust.noGravity = true;
                dust.velocity.Y -= 6f;
                dust.velocity *= 0.5f;
                dust.velocity += dustVelocity * (0.6f + 0.6f * Main.rand.NextFloat());
                increment = j;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire3, 300);
            SoundEngine.PlaySound(SoundID.Item14, target.Center);
        }

        public override void AddRecipes()
        {            
            CreateRecipe().
            AddIngredient(ItemID.FieryGreatsword).
            AddIngredient<PerennialBar>(8).
            AddTile(TileID.MythrilAnvil).
            Register();
        }
    }
}
