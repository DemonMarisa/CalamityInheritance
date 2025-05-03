using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Typeless
{
    // 用于复杂手持射弹的改变玩家朝向
    public class ChangeDirProj : BaseHeldProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override float OffsetX => 0;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        public override float AimResponsiveness => 1f;
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
    }
}
