using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            UpdateEffect_PostUpdateMisc();
            PotionBuff();
            ArmorUpdate_PostMiscUpdate();
            PostUpdate_PreKill();
            PostUpdateMiscEffect_Accessories();
            UpdateShield_PostUpdateMisc();
            MainPostUpdateMisc();
        }
    }
}
