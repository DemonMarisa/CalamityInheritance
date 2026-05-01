using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class IridescentExcalibur : GeneralWeaponClass
    {
        public override string Texture => GetInstance<ElementalExcalibur>().Texture;
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;

        private int MaxBeamTypes = 12;

        private int BeamType = 0;

        private const int Alpha = 100;

        private const float ShootSpeed = 16f;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 112;
            Item.height = 112;
            Item.damage = 3000;
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.shoot = ProjectileType<GayBeam>();
            Item.shootSpeed = ShootSpeed;
            Item.rare = RarityType<HotPink>();
            Item.shootsEveryUse = true;
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-12f * player.direction, 2f * player.gravDir).RotatedBy(player.itemRotation);
        }
        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 10;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                float adjustedItemScale = player.GetAdjustedItemScale(Item);
                Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ProjectileType<IridescentExcaliburSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
                Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ProjectileType<IridescentExcaliburSlash2>(), 0, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
                Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ProjectileType<IridescentExcaliburSlash3>(), 0, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, (int)Math.Round(damage * 0.5), knockback * 0.5f, player.whoAmI, BeamType);
                if (BeamType == 7)
                    Main.projectile[proj].penetrate = 3;

                BeamType++;
                if (BeamType >= MaxBeamTypes)
                    BeamType = 0;
            }

            return false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            Item.noMelee = player.altFunctionUse == 2;
            return base.UseItem(player);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.altFunctionUse == 2)
                modifiers.SourceDamage *= 2;
        }

        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            if (player.altFunctionUse == 2)
                modifiers.SourceDamage *= 2;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool())
            {
                Color rainbowColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Alpha);
                Color color = Color.White;
                int dustChoice = player.altFunctionUse == 2 ? Main.rand.Next(12) : BeamType;
                switch (dustChoice)
                {
                    case 0: // Red
                        color = new Color(255, 0, 0, Alpha);
                        break;
                    case 1: // Orange
                        color = new Color(255, 128, 0, Alpha);
                        break;
                    case 2: // Yellow
                        color = new Color(255, 255, 0, Alpha);
                        break;
                    case 3: // Lime
                        color = new Color(128, 255, 0, Alpha);
                        break;
                    case 4: // Green
                        color = new Color(0, 255, 0, Alpha);
                        break;
                    case 5: // Turquoise
                        color = new Color(0, 255, 128, Alpha);
                        break;
                    case 6: // Cyan
                        color = new Color(0, 255, 255, Alpha);
                        break;
                    case 7: // Light Blue
                        color = new Color(0, 128, 255, Alpha);
                        break;
                    case 8: // Blue
                        color = new Color(0, 0, 255, Alpha);
                        break;
                    case 9: // Purple
                        color = new Color(128, 0, 255, Alpha);
                        break;
                    case 10: // Fuschia
                        color = new Color(255, 0, 255, Alpha);
                        break;
                    case 11: // Hot Pink
                        color = new Color(255, 0, 128, Alpha);
                        break;
                    default:
                        break;
                }

                Dust rainbow = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.RainbowMk2)];
                rainbow.velocity *= 0f;
                rainbow.color = Color.Lerp(rainbowColor, color, (float)Math.Sqrt((MathF.Cos(Main.GlobalTimeWrappedHourly / 60f) + 1f) * 0.5f));
                rainbow.color.A = 100;
                rainbow.scale = Main.rand.NextFloat(1.2f, 1.6f);
                rainbow.fadeIn = Main.rand.NextFloat(0.4f, 1f);
                rainbow.noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<MiracleBlight>(), 600);
            target.AddBuff(BuffType<GlacialState>(), 60);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<MiracleBlight>(), 600);
            target.AddBuff(BuffType<GlacialState>(), 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient<Orderbringer>().
                AddIngredient<MiracleMatter>(3).
                AddIngredient<AshesofAnnihilation>(15).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
