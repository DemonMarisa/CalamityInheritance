using CalamityInheritance.Core.Utils;
using LAP.Core.LAPUI.CustomCD;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace CalamityInheritance.Content.CDs
{
    public class EnergyShieldDisplay : BaseCD
    {
        public int CurShield => Main.LocalPlayer.CI().stateShield;
        public int CurShieldMax => Main.LocalPlayer.CI().stateShieldMax;
        public override Rectangle OverLayerRec
        {
            get
            {
                float Progress = CurShield / ((float)CurShieldMax + 1 );
                int Begin = (int)(CDTexture_OverLayer.Height * 0.88f);
                int End = (int)(CDTexture_OverLayer.Height * 0.12f);
                int FianlCut = (int)MathHelper.Lerp(Begin, End, Progress);
                Rectangle rec = new (0, 0, CDTexture_OverLayer.Width, FianlCut);
                return rec;
            }
        }
        public override LocalizedText DisplayName() => CIUtils.GetText($"CoolDowns.EnergyShieldDisplay");
        public override void OnRegister()
        {
            Buff = false;
            DeBuff = false;
            Info = true;
        }
        public override void OnSpawn(Player player)
        {
            MaxTime = CurShieldMax;
        }
        public override void Update(Player player)
        {
        }
        public override bool PreDrawTime(DynamicSpriteFont MGRFont)
        {
            if (CurShieldMax != 0)
            {
                Time = 1;
                MaxTime = CurShieldMax;
            }
            int thisCdRemin = CurShield;
            if (thisCdRemin > CurShield)
                thisCdRemin = CurShield;
            Vector2 scale = new Vector2(0.4f);
            string Count = $"{thisCdRemin}";
            if (thisCdRemin > 999)
                scale = new Vector2(0.35f);
            if (thisCdRemin > 9999)
                scale = new Vector2(0.3f);
            Vector2 stringsize = ChatManager.GetStringSize(MGRFont, Count, Vector2.One);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, MGRFont, Count, DrawPosition + new Vector2(0, 24), Color.White, 0f, stringsize / 2, scale);
            return false;
        }
    }
}
