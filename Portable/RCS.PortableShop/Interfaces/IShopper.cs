using System.Windows.Input;

namespace RCS.PortableShop.Interfaces
{
    internal interface IShopper
    {
        // Note that set cannot be made private here.
        ICommand CartCommand { get; set; }
    }
}
