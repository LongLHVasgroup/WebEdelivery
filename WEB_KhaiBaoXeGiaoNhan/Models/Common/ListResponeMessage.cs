﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Common
{
    public class ListResponeMessage<T>
    {
        public ListResponeMessage()
        {
            data = new List<T>();
            err = new ErorrMssage();
        }

        public bool isSuccess { get; set; }
        public int totalRecords { get; set; }
        public List<T> data { get; set; }
        public ErorrMssage err { get; set; }
    }

    public class SelectItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}