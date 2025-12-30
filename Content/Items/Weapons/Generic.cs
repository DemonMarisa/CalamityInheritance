using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ID.ContentSamples.CreativeHelper;

namespace CalamityInheritance.Content.Items.Weapons
{
    public class Generic
    {
        public static string WeaponPath => "CalamityInheritance/Content/Items/Weapons";
        public static string ProjPath => "CalamityInheritance/Content/Projectiles";
        public static string BaseWeaponCategory => "Content.Items.Weapons";
        public static string WeaponTextPath => "Mods.CalamityInheritance.Content.Items.Weapons.";
        public static float GenericLegendBuff() => NPC.AnyNPCs(NPCType<SupremeCalamitasLegacy>()) ? 2.0f : 0f;
        public static int GenericLegendBuffInt(int? buffDamage = 100) => (int)(NPC.AnyNPCs(NPCType<SupremeCalamitasLegacy>()) ? buffDamage : 0);
        public static int GenericLegendBuffFloat(int? buffDamage = 5) => (int)(NPC.AnyNPCs(NPCType<SupremeCalamitasLegacy>()) ? buffDamage : 0);
    }
    public abstract class CIRogueClass : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override bool WeaponPrefix() => true;
        public override bool RangedPrefix() => false;
        public override void ModifyResearchSorting(ref ItemGroup itemGroup) => itemGroup = (ItemGroup)CalamityResearchSorting.RogueWeapon;

        public virtual float StealthDamageMultiplier => 1f;
        public virtual float StealthKnockbackMultiplier => 1f;
        public virtual bool AdditionalStealthCheck() => false;
        public override void SetDefaults()
        {
            Item.DamageType = GetInstance<RogueDamageClass>();
            ExSD();
        }
        public virtual void ExSD() { }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            bool stealthStrike = player.Calamity().StealthStrikeAvailable();
            if (stealthStrike || AdditionalStealthCheck())
            {
                damage = (int)(damage * StealthDamageMultiplier);
                knockback = knockback * StealthKnockbackMultiplier;
            }

            ModifyStatsExtra(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        //这个shoot在这里主要是为了方便快速创建，因为大部分灾厄的潜伏攻击都是在射弹处理的AI而不管shoot怎么样
        //如果需要复写，直接覆盖shoot就得了
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }
        public virtual void ModifyStatsExtra(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
    }
    public abstract class GeneralWeaponClass : ModItem, ILocalizedModType
    {
        public enum WeaponDamageType
        {
            Melee,
            Ranged,
            Magic,
            Summon,
            Typeless
        }
        public virtual WeaponDamageType UseDamageClass { get; }
        public new string LocalizationCategory => $"Content.Items.Weapons.{UseDamageClass}";
        public override void SetDefaults()
        {
            Item.DamageType = GetDamageClass();
            ExSD();
        }
        public virtual void ExSD() { }
        private DamageClass GetDamageClass()
        {
            return UseDamageClass switch
            {
                WeaponDamageType.Melee => DamageClass.Melee,
                WeaponDamageType.Ranged => DamageClass.Ranged,
                WeaponDamageType.Magic => DamageClass.Magic,
                WeaponDamageType.Summon => DamageClass.Summon,
                _ => DamageClass.Generic,
            };
        }
    }
}