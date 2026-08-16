using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.Items.Armor.ArmorBonus;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool victideSet = false;
        public bool victideSummon = false;
        public bool DesertProwler = false;
        public bool ReaverRogueSet = false;
        public bool ReaverSummoner = false;
        public bool ReaverMelee = false;
        public bool ReaverMagic = false;
        public bool ReaverRanged = false;
        public bool AncientAeroSet = false;
        public bool AncientTarragonSet = false;
        public bool AncientBloodflareSet = false;
        public bool AncientAstralSet = false;
        public bool AncientGodSlayer = false;
        public bool AncientSilvaSet = false;
        public bool GodSlayerSet = false;
        public bool GodSlayerMagic = false;
        public bool GodSlayerMelee = false;
        public bool GodSlayerRogue = false;
        public bool GodSlayerRanged = false;
        public bool GodSlayerSummon = false;
        public bool SilvaSet = false;
        public bool SilvaMagic = false;
        public bool SilvaMelee = false;
        public bool SilvaRogue = false;
        public bool SilvaRanged = false;
        public bool SilvaSummon = false;
        public void ResetArmor()
        {
            victideSet = false;
            victideSummon = false;
            DesertProwler = false;
            ReaverRogueSet = false;
            ReaverSummoner = false;
            ReaverMelee = false;
            ReaverMagic = false;
            ReaverRanged = false;
            AncientAeroSet = false;
            AncientTarragonSet = false;
            AncientBloodflareSet = false;
            AncientAstralSet = false;
            AncientGodSlayer = false;
            AncientSilvaSet = false;
            GodSlayerSet = false;
            GodSlayerMagic = false;
            GodSlayerMelee = false;
            GodSlayerRogue = false;
            GodSlayerRanged = false;
            GodSlayerSummon = false;
            SilvaMagic = false;
            SilvaMelee = false;
            SilvaRogue = false;
            SilvaRanged = false;
            SilvaSummon = false;
        }
        public void ArmorLifeRegen()
        {
            if (AncientSilvaSet)
            {
                if (Player.lifeRegen < 0)
                    AncientSilvaBonus.AncientSilvaArmor_LifeRegen(this);
                AncientSilvaBonus.AncientSilvaArmorFastRegen_LifeRegen(this);
            }
        }
        public void ArmorUpdate_PostMiscUpdate()
        {
            if (ReaverRanged)
                ReaverLegacyBonus.PostUpdate_Ranged(Player);
            if (AncientAeroSet)
                AncientAeroBonus.AncientAeroArmorBonus_PostUpdate(this, Player);
            if (GodSlayerRanged)
                GodSlayerBonus.PostUpdate_Ranged(Player);
            if (SilvaMelee)
                SilvaBonus.PostUpdate_Melee(Player);
        }
        public void ArmorModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (modifiers.DamageType.CountsAsClass(DamageClass.Ranged))
            {
                if (GodSlayerRanged)
                    GodSlayerBonus.ModifyHitNPC_Ranged(Player, ref modifiers);
            }
            else if (modifiers.DamageType.IsTrueMelee())
            {
                if (SilvaMelee)
                    SilvaBonus.ModifyHitNPC_Melee(target, ref modifiers);
            }
        }
        public void ArmorOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (AncientBloodflareSet)
                AncientBloodflare.AncientBloodFlareArmorHitNPC(this, target);
            if (AncientAstralSet)
                AncientAstralBonus.AncientAstralArmorBonus_OnHitNPC(this, Player, hit);
            if (hit.DamageType.CountsAsClass(DamageClass.Melee))
            {
                if (GodSlayerMelee)
                    GodSlayerBonus.HitNPC_Melee(this, hit, target);
            }
            else if (hit.DamageType.CountsAsClass(DamageClass.Magic))
            {
                if (SilvaMagic)
                    SilvaBonus.HitNPC_Magic(this, hit, target);
            }
        }
        public void ArmorOnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (SilvaSet)
                SilvaBonus.HitNPCWithProj_All(this, proj, hit, target);
            if (proj.DamageType.CountsAsClass(DamageClass.Melee))
            {
                if (ReaverMelee)
                    ReaverLegacyBonus.HitNPCWithProj_Melee(Player,proj, target);
            }
            else if (proj.DamageType.CountsAsClass(DamageClass.Ranged))
            {
                if (DesertProwler)
                    DesertProwlerBonus.DesertProwlerArmorBonus_OnHitNPCProj(this, Player, proj, target, hit, damageDone);
            }
            else if (proj.DamageType.CountsAsClass(DamageClass.Magic))
            {
                if (ReaverMagic)
                    ReaverLegacyBonus.HitNPCWithProj_Magic(Player, proj, target);
                if (GodSlayerMagic)
                    GodSlayerBonus.HitNPCWithProj_Magic(this, proj, hit, target);
            }
            else if (proj.DamageType.CountsAsClass(DamageClass.Summon))
            {
                if (GodSlayerSummon)
                    GodSlayerBonus.HitNPCWithProj_Summon(this, proj, hit, target);
            }
        }
        public void ArmorOnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.DamageType.CountsAsClass(DamageClass.Melee))
            {
                if (ReaverMelee)
                    ReaverLegacyBonus.HitNPCWithItem_Melee(Player, target, hit);
            }
        }
        public void ArmorOnHurt(Player.HurtInfo info)
        {
            if (ReaverMelee)
                ReaverLegacyBonus.OnPlayerHurt_Melee(Player);
            if (AncientAeroSet)
                AncientAeroBonus.AncientAeroArmorHurt(this);
            if (AncientBloodflareSet)
                AncientBloodflare.AncientBloodFlareArmorHurt(this, info);
            if (AncientAstralSet)
                AncientAstralBonus.AncientAstralArmorBonus_OnHurt(this);
            if (AncientGodSlayer)
                AncientGodSlayerBonus.AncientGodSlayerArmor_Hurt(this);
            if (AncientSilvaSet)
                AncientSilvaBonus.AncientSilvaArmor_Hurt(Player, info);
            if (GodSlayerMagic)
                GodSlayerBonus.OnHurt_Magic(Player);
            if (GodSlayerMelee)
                GodSlayerBonus.OnHurt_Melee(Player);
        }
    }
}
