using System.Collections;
using System.Collections.Generic;
using ObserverPattern;

namespace Messenger
{
    public interface IMessenger : IObservable, IObserver
    {
        public void GetDictionary();
    }

}
