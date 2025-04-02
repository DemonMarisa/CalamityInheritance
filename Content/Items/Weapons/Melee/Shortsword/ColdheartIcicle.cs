using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class ColdheartIcicle : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.33f;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Melee;
            Item.width = 26;
            Item.height = 26;
            Item.useAnimation = Item.useTime = 12;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.useTurn = true;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPricePink;
            Item.shoot = ModContent.ProjectileType<ColdheartIcicleProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Pink;
        }
        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SetMaxDamage(target.statLifeMax2 * 2 / 100);
            target.statDefense -= (int)target.statDefense;
            target.endurance = 0f;
        }
        public override bool MeleePrefix() => true;
        // LATER -- Providence specifically is immune to Coldheart Icicle. There is probably a better way to do this
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
        }
    }
}
