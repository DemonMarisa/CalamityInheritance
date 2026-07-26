using CalamityInheritance.Common.Blance;
using CalamityInheritance.Content.Items.Weapons.Melee.ArkWeapons;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPUI.CustomCD;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace CalamityInheritance.Content.CDs
{
    public class AOTCCharge : BaseCD
    {
        public int CurAOTCCharge => Main.LocalPlayer.CI().CurAOTCCharge;
        public int MaxAOTCCharge => CIWeaponsBlance.MaxAOTCCharge;
        public override Rectangle OverLayerRec => new Rectangle(0, 0, CDTexture_OverLayer.Width, (int)(CDTexture_OverLayer.Height - CDTexture_OverLayer.Height * (CurAOTCCharge / (float)MaxAOTCCharge)));
        public override LocalizedText DisplayName() => CIUtils.GetText($"CoolDowns.AOTCCharge");
        public override void OnRegister()
        {
            Buff = false;
            DeBuff = false;
            Info = true;
        }
        public override void OnSpawn(Player player)
        {
            MaxTime = CIWeaponsBlance.MaxAOTCCharge;
        }
        public override void Update(Player player)
        {
            if (player.HeldItem.type == ItemType<ArkoftheCosmosNew>())
            {
                Time = 2;
                MaxTime = CIWeaponsBlance.MaxAOTCCharge;
            }
        }
        public override bool PreDrawTime(DynamicSpriteFont MGRFont)
        {
            int thisCdRemin = CurAOTCCharge;
            if (thisCdRemin > CIWeaponsBlance.MaxAOTCCharge)
                thisCdRemin = CIWeaponsBlance.MaxAOTCCharge;
            string Count = $"{thisCdRemin}";
            Vector2 stringsize = ChatManager.GetStringSize(MGRFont, Count, Vector2.One);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, MGRFont, Count, DrawPosition + new Vector2(0, 24), Color.White, 0f, stringsize / 2, new Vector2(0.4f));
            return false;
        }
    }
}
