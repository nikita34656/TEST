using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Services
{
    public static class NavigationService
    {
        public static event Action<object> NavigateTo;

        public static void Navigate(object viewModel)
        {
            NavigateTo?.Invoke(viewModel);
        }
    }
}
