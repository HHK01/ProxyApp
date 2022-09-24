namespace ProxyApp.Model
{
    public class Endpoint
    {
        /// <summary>
        /// The name or definition of the swagger endpoint, this name will show when toggling between endpoints in the swagger UI
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The url or relative endpoint path 
        /// </summary>
        public string ProxyUrlPath { get; set; }
        
        /// <summary>
        /// The swagger definition of your backend layer
        /// </summary>
        public string BackendSwaggerEndpoint { get; set; }

        /// <summary>
        /// The Base url of the backend endpoint
        /// </summary>
        public string BackendBaseEndpoint { get; set; }
    }
}
