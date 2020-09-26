using System.Threading.Tasks;

namespace DSG.Presentation.Services
{
    public interface INavigate
    {
        Task OnPageLoadedAsync(params object[] data);
    }
}
