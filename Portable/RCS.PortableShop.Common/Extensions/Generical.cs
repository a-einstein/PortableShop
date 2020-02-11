using System;

namespace RCS.PortableShop.Common.Extensions
{
    public static class Generical
    {
        // Based on katbyte's solution in https://stackoverflow.com/questions/872323/method-call-if-not-null-in-c-sharp
        public static void IfNotNull<T>(this T thisObject, Action<T> actionNotNull, Action actionNull = null) where T : class
        {
            if (thisObject != null)
            {
                if (actionNotNull == null)
                    throw new ArgumentNullException(nameof(actionNotNull));

                actionNotNull(thisObject);
            }
            else
            {
                actionNull?.Invoke();
            }
        }
    }
}
