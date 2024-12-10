using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class AEvent : IEvent
    {
        public Action Action{get; set;}
    }

}
