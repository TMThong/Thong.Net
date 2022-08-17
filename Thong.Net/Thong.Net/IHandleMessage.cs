﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thong.Net
{
    public interface IHandleMessage
    {
         void OnHandle(Message message);
         void OnDisconected();
    }
}
