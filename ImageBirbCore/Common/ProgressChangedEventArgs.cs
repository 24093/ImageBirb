using System;

namespace ImageBirb.Core.Common
{
    public class ProgressChangedEventArgs : EventArgs
    {
        public int Progress { get; }

        public int Max { get; }

        public ProgressType Type { get; }

        public ProgressChangedEventArgs(ProgressType type, int progress, int max = 100)
        {
            Type = type;
            Progress = progress;
            Max = max;
        }
    }
}