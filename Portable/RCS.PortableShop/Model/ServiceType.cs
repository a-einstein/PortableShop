namespace RCS.PortableShop.Model
{
    // Order is simply rearranged to desired one, as this does not disrupt.
    // Assigning IDs preserves currently stored values, but does not enable rearranging.
    public enum ServiceType
    {
        WebApi,
        WCF,
        CoreWcf
    }
}