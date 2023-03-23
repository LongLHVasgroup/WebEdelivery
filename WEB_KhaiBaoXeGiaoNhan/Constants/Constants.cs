namespace WEB_KhaiBaoXeGiaoNhan.Constants
{
    public sealed class UserConstants
    {
        public static string UserTypeC = "Customer";
        public static string UserTypeP = "Provider";
        public static string UserTypeCor = "Corrdinator";
    }

    public sealed class GiaoNhan
    {
        public static string giao = "giao";
        public static string nhan = "nhan";
    }

    public sealed class CungDuongDefault
    {
        public static string plant3000 = "Cung đường khác";
        public static string plant4000 = "Cung đường khác";
        public static string plant6000 = "Khác";
    }

    public sealed class CungDuong
    {
        public static readonly string[] SearchCriteria = { "Khác", "Cung Đường Khác" };
    }

    public sealed class PoProperties
    {
        public static readonly string[] PoDB = { "ĐB", "DB" };

        public static bool CheckOrtherPrice(string s)
        {
            s = s.Replace(" ", "");
            for (int i = 0; i < PoDB.Length; i++)
            {
                if (PoDB[i].Equals(s.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public sealed class PlantConstants{
        public static string P1000 ="1000";
        public static string P3000 ="3000";
        public static string P4000 ="4000";
        public static string P6000 ="6000";
        public static string P7000 ="7000";
    }

    // Đánh dấu PO nhập đường thủy không cần hiện lên cho NCC khai báo
    public sealed class PODuongThuyConstants
    {
        public static string PODUONGTHUY = "PO ĐƯỜNG THỦY";
    }
}