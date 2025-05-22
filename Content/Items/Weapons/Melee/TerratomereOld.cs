using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TerratomereOld : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.BonusAttackSpeedMultiplier[Item.type] = 1.2f;
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.damage = 280;
            Item.DamageType = DamageClass.Melee;
            Item.scale = 1f;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 21;
            Item.knockBack = 7f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.shoot = ModContent.ProjectileType<TerratomereProjectile>();
            Item.shootSpeed = 20f;
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.scale = 0.6f;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ModContent.ProjectileType<TerratomereProjectile>();
            }
            else
            {
                Item.scale = 1f;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ModContent.ProjectileType<TerratomereProjectile>();
            }
            return true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (Main.zenithWorld)
                damage.Base *= 0.3f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 4; ++index)
            {
                Projectile.NewProjectile(source, player.Center, velocity.RotatedByRandom(MathHelper.ToRadians(5f)), type, (int)(damage * 0.5), knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 30);

            if (!target.canGhostHeal || player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 30);

            if (player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void AddRecipes()
        {

            CreateRecipe().
                AddIngredient<Floodtide>().
                AddIngredient<Hellkite>().
                AddRecipeGroup(CIRecipeGroup.TerraBlade).
                AddIngredient<UelibloomBar>(7).
                AddCondition(Condition.NotZenithWorld).
                AddDecraftCondition(Condition.NotZenithWorld).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient<TrueNightsStabber>().
                AddIngredient<TrueExcaliburShortsword>().
                AddIngredient<LivingShard>(5).
                AddIngredient(ItemID.BrokenHeroSword).
                AddCondition(Condition.ZenithWorld).
                AddDecraftCondition(Condition.ZenithWorld).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.PiercingStarlight, 1).
                AddIngredient<LivingShard>(5).
                AddCondition(Condition.ZenithWorld).
                DisableDecraft().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
