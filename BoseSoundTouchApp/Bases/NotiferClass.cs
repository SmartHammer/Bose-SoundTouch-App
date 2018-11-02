using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BoseSoundTouchApp.Bases
{
    public class NotiferClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            catch(Exception)
            { }
        }
    }
}
