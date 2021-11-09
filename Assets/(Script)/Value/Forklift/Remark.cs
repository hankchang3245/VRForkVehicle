﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace edu.tnu.dgd.value
{
    [Serializable]
    public class Remark
    {
        public string text;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}