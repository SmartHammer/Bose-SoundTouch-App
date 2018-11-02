using BoseSoundTouchApp.Models;

namespace BoseSoundTouchApp.Services
{
    public class GET_info : GET, IInfo
    {
        public GET_info(ref IPhysicalData physicalData)
            : base(ref physicalData)
        { }

        public string deviceID => throw new System.NotImplementedException();

        public string name => throw new System.NotImplementedException();

        public string type => throw new System.NotImplementedException();

        public IComponent component => throw new System.NotImplementedException();

        public string margeAccountUUID => throw new System.NotImplementedException();

        public INetworkInfo networkInfo => throw new System.NotImplementedException();

        public string moduleType => throw new System.NotImplementedException();

        public string variant => throw new System.NotImplementedException();

        public string variantMode => throw new System.NotImplementedException();

        public string countryCode => throw new System.NotImplementedException();

        public string regionCode => throw new System.NotImplementedException();
    }
}
