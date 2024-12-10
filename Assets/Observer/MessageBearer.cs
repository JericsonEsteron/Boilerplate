using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

namespace Messenger
{
    public class MessageBearer
    {
        public static readonly IMessenger Default = new Observer();
    }

}
