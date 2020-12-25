using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Bcm
{
    public interface IBcm
    {
        [FunctionName("bcm_host_init")]
        void HostInit();
    }
}
