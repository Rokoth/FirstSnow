using System.Threading.Tasks;

namespace FirstSnow.FirstSnowDeployer
{
    public interface IDeployService
    {
        Task Deploy(int? num = null);
    }
}
