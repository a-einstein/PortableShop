using System.Windows.Input;

namespace RCS.PortableShop.Interfaces
{
    interface IShopper
    {
        // Note that set cannot be made private here.
        ICommand CartCommand { get; set; }
    }
}
