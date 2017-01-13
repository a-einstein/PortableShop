﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RCS.PortableShop.ServiceClients.Products.ProductsService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ProductsOverviewList", Namespace="http://schemas.datacontract.org/2004/07/RCS.AdventureWorks.Common.Dtos", ItemName="ProductsOverviewObject")]
    public class ProductsOverviewList : System.Collections.ObjectModel.ObservableCollection<RCS.AdventureWorks.Common.DomainClasses.ProductsOverviewObject> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ProductCategoryList", Namespace="http://schemas.datacontract.org/2004/07/RCS.AdventureWorks.Common.Dtos", ItemName="ProductCategory")]
    public class ProductCategoryList : System.Collections.ObjectModel.ObservableCollection<RCS.AdventureWorks.Common.DomainClasses.ProductCategory> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ProductSubcategoryList", Namespace="http://schemas.datacontract.org/2004/07/RCS.AdventureWorks.Common.Dtos", ItemName="ProductSubcategory")]
    public class ProductSubcategoryList : System.Collections.ObjectModel.ObservableCollection<RCS.AdventureWorks.Common.DomainClasses.ProductSubcategory> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ProductsService.IProductsService")]
    public interface IProductsService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IProductsService/GetProductsOverviewBy", ReplyAction="http://tempuri.org/IProductsService/GetProductsOverviewByResponse")]
        System.IAsyncResult BeginGetProductsOverviewBy(System.Nullable<int> productCategoryID, System.Nullable<int> productSubcategoryID, string productNameString, System.AsyncCallback callback, object asyncState);
        
        RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList EndGetProductsOverviewBy(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IProductsService/GetProductDetails", ReplyAction="http://tempuri.org/IProductsService/GetProductDetailsResponse")]
        System.IAsyncResult BeginGetProductDetails(int productId, System.AsyncCallback callback, object asyncState);
        
        RCS.AdventureWorks.Common.DomainClasses.Product EndGetProductDetails(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IProductsService/GetProductCategories", ReplyAction="http://tempuri.org/IProductsService/GetProductCategoriesResponse")]
        System.IAsyncResult BeginGetProductCategories(System.AsyncCallback callback, object asyncState);
        
        RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList EndGetProductCategories(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IProductsService/GetProductSubcategories", ReplyAction="http://tempuri.org/IProductsService/GetProductSubcategoriesResponse")]
        System.IAsyncResult BeginGetProductSubcategories(System.AsyncCallback callback, object asyncState);
        
        RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList EndGetProductSubcategories(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProductsServiceChannel : RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProductsOverviewByCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProductsOverviewByCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProductDetailsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProductDetailsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public RCS.AdventureWorks.Common.DomainClasses.Product Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((RCS.AdventureWorks.Common.DomainClasses.Product)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProductCategoriesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProductCategoriesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProductSubcategoriesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProductSubcategoriesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProductsServiceClient : System.ServiceModel.ClientBase<RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService>, RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService {
        
        private BeginOperationDelegate onBeginGetProductsOverviewByDelegate;
        
        private EndOperationDelegate onEndGetProductsOverviewByDelegate;
        
        private System.Threading.SendOrPostCallback onGetProductsOverviewByCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetProductDetailsDelegate;
        
        private EndOperationDelegate onEndGetProductDetailsDelegate;
        
        private System.Threading.SendOrPostCallback onGetProductDetailsCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetProductCategoriesDelegate;
        
        private EndOperationDelegate onEndGetProductCategoriesDelegate;
        
        private System.Threading.SendOrPostCallback onGetProductCategoriesCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetProductSubcategoriesDelegate;
        
        private EndOperationDelegate onEndGetProductSubcategoriesDelegate;
        
        private System.Threading.SendOrPostCallback onGetProductSubcategoriesCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ProductsServiceClient() : 
                base(ProductsServiceClient.GetDefaultBinding(), ProductsServiceClient.GetDefaultEndpointAddress()) {
        }
        
        public ProductsServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(ProductsServiceClient.GetBindingForEndpoint(endpointConfiguration), ProductsServiceClient.GetEndpointAddress(endpointConfiguration)) {
        }
        
        public ProductsServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ProductsServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress)) {
        }
        
        public ProductsServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ProductsServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress) {
        }
        
        public ProductsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetProductsOverviewByCompletedEventArgs> GetProductsOverviewByCompleted;
        
        public event System.EventHandler<GetProductDetailsCompletedEventArgs> GetProductDetailsCompleted;
        
        public event System.EventHandler<GetProductCategoriesCompletedEventArgs> GetProductCategoriesCompleted;
        
        public event System.EventHandler<GetProductSubcategoriesCompletedEventArgs> GetProductSubcategoriesCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.BeginGetProductsOverviewBy(System.Nullable<int> productCategoryID, System.Nullable<int> productSubcategoryID, string productNameString, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.EndGetProductsOverviewBy(System.IAsyncResult result) {
            return base.Channel.EndGetProductsOverviewBy(result);
        }
        
        private System.IAsyncResult OnBeginGetProductsOverviewBy(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Nullable<int> productCategoryID = ((System.Nullable<int>)(inValues[0]));
            System.Nullable<int> productSubcategoryID = ((System.Nullable<int>)(inValues[1]));
            string productNameString = ((string)(inValues[2]));
            return ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).BeginGetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString, callback, asyncState);
        }
        
        private object[] OnEndGetProductsOverviewBy(System.IAsyncResult result) {
            RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList retVal = ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).EndGetProductsOverviewBy(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProductsOverviewByCompleted(object state) {
            if ((this.GetProductsOverviewByCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProductsOverviewByCompleted(this, new GetProductsOverviewByCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProductsOverviewByAsync(System.Nullable<int> productCategoryID, System.Nullable<int> productSubcategoryID, string productNameString) {
            this.GetProductsOverviewByAsync(productCategoryID, productSubcategoryID, productNameString, null);
        }
        
        public void GetProductsOverviewByAsync(System.Nullable<int> productCategoryID, System.Nullable<int> productSubcategoryID, string productNameString, object userState) {
            if ((this.onBeginGetProductsOverviewByDelegate == null)) {
                this.onBeginGetProductsOverviewByDelegate = new BeginOperationDelegate(this.OnBeginGetProductsOverviewBy);
            }
            if ((this.onEndGetProductsOverviewByDelegate == null)) {
                this.onEndGetProductsOverviewByDelegate = new EndOperationDelegate(this.OnEndGetProductsOverviewBy);
            }
            if ((this.onGetProductsOverviewByCompletedDelegate == null)) {
                this.onGetProductsOverviewByCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProductsOverviewByCompleted);
            }
            base.InvokeAsync(this.onBeginGetProductsOverviewByDelegate, new object[] {
                        productCategoryID,
                        productSubcategoryID,
                        productNameString}, this.onEndGetProductsOverviewByDelegate, this.onGetProductsOverviewByCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.BeginGetProductDetails(int productId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProductDetails(productId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RCS.AdventureWorks.Common.DomainClasses.Product RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.EndGetProductDetails(System.IAsyncResult result) {
            return base.Channel.EndGetProductDetails(result);
        }
        
        private System.IAsyncResult OnBeginGetProductDetails(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int productId = ((int)(inValues[0]));
            return ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).BeginGetProductDetails(productId, callback, asyncState);
        }
        
        private object[] OnEndGetProductDetails(System.IAsyncResult result) {
            RCS.AdventureWorks.Common.DomainClasses.Product retVal = ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).EndGetProductDetails(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProductDetailsCompleted(object state) {
            if ((this.GetProductDetailsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProductDetailsCompleted(this, new GetProductDetailsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProductDetailsAsync(int productId) {
            this.GetProductDetailsAsync(productId, null);
        }
        
        public void GetProductDetailsAsync(int productId, object userState) {
            if ((this.onBeginGetProductDetailsDelegate == null)) {
                this.onBeginGetProductDetailsDelegate = new BeginOperationDelegate(this.OnBeginGetProductDetails);
            }
            if ((this.onEndGetProductDetailsDelegate == null)) {
                this.onEndGetProductDetailsDelegate = new EndOperationDelegate(this.OnEndGetProductDetails);
            }
            if ((this.onGetProductDetailsCompletedDelegate == null)) {
                this.onGetProductDetailsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProductDetailsCompleted);
            }
            base.InvokeAsync(this.onBeginGetProductDetailsDelegate, new object[] {
                        productId}, this.onEndGetProductDetailsDelegate, this.onGetProductDetailsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.BeginGetProductCategories(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProductCategories(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.EndGetProductCategories(System.IAsyncResult result) {
            return base.Channel.EndGetProductCategories(result);
        }
        
        private System.IAsyncResult OnBeginGetProductCategories(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).BeginGetProductCategories(callback, asyncState);
        }
        
        private object[] OnEndGetProductCategories(System.IAsyncResult result) {
            RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList retVal = ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).EndGetProductCategories(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProductCategoriesCompleted(object state) {
            if ((this.GetProductCategoriesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProductCategoriesCompleted(this, new GetProductCategoriesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProductCategoriesAsync() {
            this.GetProductCategoriesAsync(null);
        }
        
        public void GetProductCategoriesAsync(object userState) {
            if ((this.onBeginGetProductCategoriesDelegate == null)) {
                this.onBeginGetProductCategoriesDelegate = new BeginOperationDelegate(this.OnBeginGetProductCategories);
            }
            if ((this.onEndGetProductCategoriesDelegate == null)) {
                this.onEndGetProductCategoriesDelegate = new EndOperationDelegate(this.OnEndGetProductCategories);
            }
            if ((this.onGetProductCategoriesCompletedDelegate == null)) {
                this.onGetProductCategoriesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProductCategoriesCompleted);
            }
            base.InvokeAsync(this.onBeginGetProductCategoriesDelegate, null, this.onEndGetProductCategoriesDelegate, this.onGetProductCategoriesCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.BeginGetProductSubcategories(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProductSubcategories(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService.EndGetProductSubcategories(System.IAsyncResult result) {
            return base.Channel.EndGetProductSubcategories(result);
        }
        
        private System.IAsyncResult OnBeginGetProductSubcategories(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).BeginGetProductSubcategories(callback, asyncState);
        }
        
        private object[] OnEndGetProductSubcategories(System.IAsyncResult result) {
            RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList retVal = ((RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService)(this)).EndGetProductSubcategories(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProductSubcategoriesCompleted(object state) {
            if ((this.GetProductSubcategoriesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProductSubcategoriesCompleted(this, new GetProductSubcategoriesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProductSubcategoriesAsync() {
            this.GetProductSubcategoriesAsync(null);
        }
        
        public void GetProductSubcategoriesAsync(object userState) {
            if ((this.onBeginGetProductSubcategoriesDelegate == null)) {
                this.onBeginGetProductSubcategoriesDelegate = new BeginOperationDelegate(this.OnBeginGetProductSubcategories);
            }
            if ((this.onEndGetProductSubcategoriesDelegate == null)) {
                this.onEndGetProductSubcategoriesDelegate = new EndOperationDelegate(this.OnEndGetProductSubcategories);
            }
            if ((this.onGetProductSubcategoriesCompletedDelegate == null)) {
                this.onGetProductSubcategoriesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProductSubcategoriesCompleted);
            }
            base.InvokeAsync(this.onBeginGetProductSubcategoriesDelegate, null, this.onEndGetProductSubcategoriesDelegate, this.onGetProductSubcategoriesCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService CreateChannel() {
            return new ProductsServiceClientChannel(this);
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IProductsService)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.MaxReceivedMessageSize = int.MaxValue;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IProductsService)) {
                return new System.ServiceModel.EndpointAddress("http://localhost:65348/ProductsService.svc/ProductsService");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding() {
            return ProductsServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IProductsService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress() {
            return ProductsServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IProductsService);
        }
        
        private class ProductsServiceClientChannel : ChannelBase<RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService>, RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService {
            
            public ProductsServiceClientChannel(System.ServiceModel.ClientBase<RCS.PortableShop.ServiceClients.Products.ProductsService.IProductsService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetProductsOverviewBy(System.Nullable<int> productCategoryID, System.Nullable<int> productSubcategoryID, string productNameString, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = productCategoryID;
                _args[1] = productSubcategoryID;
                _args[2] = productNameString;
                System.IAsyncResult _result = base.BeginInvoke("GetProductsOverviewBy", _args, callback, asyncState);
                return _result;
            }
            
            public RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList EndGetProductsOverviewBy(System.IAsyncResult result) {
                object[] _args = new object[0];
                RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList _result = ((RCS.PortableShop.ServiceClients.Products.ProductsService.ProductsOverviewList)(base.EndInvoke("GetProductsOverviewBy", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetProductDetails(int productId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = productId;
                System.IAsyncResult _result = base.BeginInvoke("GetProductDetails", _args, callback, asyncState);
                return _result;
            }
            
            public RCS.AdventureWorks.Common.DomainClasses.Product EndGetProductDetails(System.IAsyncResult result) {
                object[] _args = new object[0];
                RCS.AdventureWorks.Common.DomainClasses.Product _result = ((RCS.AdventureWorks.Common.DomainClasses.Product)(base.EndInvoke("GetProductDetails", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetProductCategories(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetProductCategories", _args, callback, asyncState);
                return _result;
            }
            
            public RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList EndGetProductCategories(System.IAsyncResult result) {
                object[] _args = new object[0];
                RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList _result = ((RCS.PortableShop.ServiceClients.Products.ProductsService.ProductCategoryList)(base.EndInvoke("GetProductCategories", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetProductSubcategories(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetProductSubcategories", _args, callback, asyncState);
                return _result;
            }
            
            public RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList EndGetProductSubcategories(System.IAsyncResult result) {
                object[] _args = new object[0];
                RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList _result = ((RCS.PortableShop.ServiceClients.Products.ProductsService.ProductSubcategoryList)(base.EndInvoke("GetProductSubcategories", _args, result)));
                return _result;
            }
        }
        
        public enum EndpointConfiguration {
            
            BasicHttpBinding_IProductsService,
        }
    }
}