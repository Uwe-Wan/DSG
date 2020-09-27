using System.Threading.Tasks;

namespace DSG.Presentation.Services
{
    public interface INaviService
    {
        Task NavigateToAsync(NavigationDestination destination, params object[] data);
    }
}
