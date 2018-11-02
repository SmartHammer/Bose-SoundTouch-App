using BoseSoundTouchApp.Bases;
using BoseSoundTouchApp.Models;
using System.ComponentModel;

namespace BoseSoundTouchApp.ViewModels
{
    public class DeviceSelectionViewModel : NotiferClass
    {
        #region Members
        private const string ALL = "All";
        private const string CURRENT = "CurrentDevice";
        #endregion Members

        public IGeneralModel Model
        {
            private get;
            set;
        }

        public void Notifier(object sender, PropertyChangedEventArgs e)
        {

        }
    }
}
