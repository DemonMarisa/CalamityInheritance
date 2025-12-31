using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class GrandGuardianLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        internal const int TotalHealOrbs = 3;

        internal const int HealPerOrb = 3;

        internal const int TotalHealed = TotalHealOrbs * HealPerOrb;
        public override void SetDefaults()
        {
            Item.width = 130;
            Item.height = 130;
            Item.damage = 150;
            Item.DamageType = GetInstance<TrueMeleeDamageClass>();
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 12f;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-12f * player.direction, 2f * player.gravDir).RotatedBy(player.itemRotation);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.Calamity().miscDefenseLoss < target.defense)
                target.Calamity().miscDefenseLoss += 1;

            OnHitEffects(player, target.Center, target.life, target.lifeMax, Item.knockBack, Item.damage);

            if (target.Calamity().miscDefenseLoss >= target.defense)
            {
                if (player.moonLeech || player.lifeSteal <= 0f || target.lifeMax <= 5)
                    return;

                int heal = 6;
                player.lifeSteal -= heal;
                player.statLife += heal;
                player.HealEffect(heal);
                if (player.statLife > player.statLifeMax2)
                    player.statLife = player.statLifeMax2;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            OnHitEffects(player, target.Center, target.statLife, target.statLifeMax2, Item.knockBack, Item.damage);
        }

        private void OnHitEffects(Player player, Vector2 targetPos, int targetLife, int targetMaxLife, float knockback, int damage)
        {
            var source = player.GetSource_ItemUse(Item);

            // Grand Guardian is classed as a regular melee weapon, so despite being a true melee on-hit, these scale with regular melee.
            StatModifier playerMeleeDmg = player.GetTotalDamage<MeleeDamageClass>();
            int rainbowBoomDamage = (int)playerMeleeDmg.ApplyTo(damage * 0.5f);
            int rainBoltDamage = (int)playerMeleeDmg.ApplyTo(damage * 0.75f);

            Projectile.NewProjectile(source, targetPos, Vector2.Zero, ProjectileType<GrandGuardianBoom>(), rainbowBoomDamage, 0f, player.whoAmI);

            if (targetLife <= (targetMaxLife * 0.5f) && player.ownedProjectileCounts[ProjectileType<GrandGuardianBolt>()] < 3)
            {
                float randomSpeedX = Main.rand.NextFloat(6f, 12f);
                float randomSpeedY = Main.rand.NextFloat(6f, 12f);
                Projectile.NewProjectile(source, targetPos.X, targetPos.Y, -randomSpeedX, -randomSpeedY, ProjectileType<GrandGuardianBolt>(), rainBoltDamage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, targetPos.X, targetPos.Y, randomSpeedX, -randomSpeedY, ProjectileType<GrandGuardianBolt>(), rainBoltDamage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, targetPos.X, targetPos.Y, 0f, -randomSpeedY, ProjectileType<GrandGuardianBolt>(), rainBoltDamage, knockback, player.whoAmI);
            }

            if (player.moonLeech || player.lifeSteal <= 0f)
                return;

            if (targetLife <= 0 && !player.moonLeech && player.ownedProjectileCounts[ProjectileType<GrandGuardianHeal>()] < 3 && targetMaxLife > 5)
            {
                player.lifeSteal -= TotalHealed;
                float randomSpeedX = Main.rand.NextFloat(3f, 4.5f);
                float randomSpeedY = Main.rand.NextFloat(3f, 4.5f);
                Projectile.NewProjectile(source, targetPos.X, targetPos.Y, -randomSpeedX, -randomSpeedY, ProjectileType<GrandGuardianHeal>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(source, targetPos.X, targetPos.Y, randomSpeedX, -randomSpeedY, ProjectileType<GrandGuardianHeal>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(source, targetPos.X, targetPos.Y, 0f, -randomSpeedY, ProjectileType<GrandGuardianHeal>(), 0, 0f, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MajesticGuard>().
                AddIngredient<BurntSienna>().
                AddIngredient(ItemID.FragmentNebula, 6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
