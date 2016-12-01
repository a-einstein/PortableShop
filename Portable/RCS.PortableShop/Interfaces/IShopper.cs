using System.Windows.Input;

namespace RCS.PortableShop.Interfaces
{
    interface IShopper
    {
        ICommand CartCommand { get; set; }
    }
}
