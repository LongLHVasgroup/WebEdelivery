using System;

namespace WEB_KhaiBaoXeGiaoNhan
{
    public class MergeDataResult
    {
        public string Source { get; set; }

        public int Total { get; set; }

        public int Updated { get; set; }

        public int Inserted { get; set; }

        public string Model { get; set; }
    }
}
