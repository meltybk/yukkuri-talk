using System;
using System.Runtime.InteropServices;

namespace YukkuriTalk
{
    internal class AquesTalk
    {
        [DllImport("D:\\yukkuri-talk\\AquesTalk.dll")]
        public static extern IntPtr AquesTalk_Synthe(string koe, int iSpeed, ref int size);

        [DllImport("D:\\yukkuri-talk\\AquesTalk.dll")]
        public static extern void AquesTalk_FreeWave(IntPtr wavPtr);

        [DllImport("D:\\yukkuri-talk\\AquesTalk.dll")]
        public static extern int AquesTalkDa_PlaySync(IntPtr koe, int iSpeed);
    }
}
