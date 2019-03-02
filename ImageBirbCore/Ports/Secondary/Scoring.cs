using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    internal class Scoring
    {
        public delegate Task<double> ScoreFunc(string fingerprint1, string fingerprint2);
    }
}