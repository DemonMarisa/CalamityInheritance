using CalamityInheritance.Content.Items.Weapons.Ranged.Bows;
using CalamityInheritance.Content.Projectiles.Melee.Lasers;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Specials
{
    public class PrismaticBreakerLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        internal static readonly Color[] colors = new Color[]
        {
            new Color(255, 0, 0, 50), //Red
            new Color(255, 128, 0, 50), //Orange
            new Color(255, 255, 0, 50), //Yellow
            new Color(128, 255, 0, 50), //Lime
            new Color(0, 255, 0, 50), //Green
            new Color(0, 255, 128, 50), //Turquoise
            new Color(0, 255, 255, 50), //Cyan
            new Color(0, 128, 255, 50), //Light Blue
            new Color(0, 0, 255, 50), //Blue
            new Color(128, 0, 255, 50), //Purple
            new Color(255, 0, 255, 50), //Fuschia
            new Color(255, 0, 128, 50) //Hot Pink
        };

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;

            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 699;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 13;
            Item.useTurn = false;
            Item.DamageType = MeleeRangedHybridDamageClass.Instance;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PrismaticBeamLegacy>();
            Item.shootSpeed = 14f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.Calamity().donorItem = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 8;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<PrismaticWaveLegacy>(), damage, knockback, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity * 0.5f, type, (int)(damage * 1.1f), knockback, player.whoAmI);
            }
            return false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.UseSound = SoundID.Item1;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.useTurn = true;
                Item.autoReuse = true;
                Item.noMelee = false;
                Item.channel = false;
            }
            else
            {
                Item.UseSound = CrystylCrusher.ChargeSound;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.useTurn = false;
                Item.autoReuse = false;
                Item.noMelee = true;
                Item.channel = true;
            }
            return base.CanUseItem(player);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                Dust rainbow = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.RainbowMk2, 0f, 0f, 50, Main.rand.Next(colors), 0.8f)];
                rainbow.noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Nightwither>(), 300);
            target.AddBuff(BuffID.Daybreak, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<Nightwither>(), 300);
            target.AddBuff(BuffID.Daybreak, 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmicRainbowLegacy>().
                AddIngredient<SolsticeClaymore>().
                AddIngredient<LifeAlloy>(3).
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<EndothermicEnergy>(20).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
