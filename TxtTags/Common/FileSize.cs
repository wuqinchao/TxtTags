using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags.Common
{
    public enum DataSizeType
    {
        b, B, K, M, G, T, P, E, Z
    }
    public class FileSize
    {
        private const decimal B = 8;
        private const decimal K = 8 * 1024m;
        private const decimal M = 8 * 1024m * 1024m;
        private const decimal G = 8 * 1024m * 1024m * 1024m;
        private const decimal T = 8 * 1024m * 1024m * 1024m * 1024m;
        private const decimal P = 8 * 1024m * 1024m * 1024m * 1024m * 1024m;
        private const decimal E = 8 * 1024m * 1024m * 1024m * 1024m * 1024m * 1024m;
        private const decimal Z = 8 * 1024m * 1024m * 1024m * 1024m * 1024m * 1024m * 1024m;

        private static DataSizeType UpgradeSizeType(DataSizeType type)
        {
            return type == DataSizeType.Z ? DataSizeType.Z : (type + 1);
        }
        public static decimal getSizeString(object source, DataSizeType sourceType, DataSizeType targetType)
        {
            decimal sv = Convert.ToDecimal(source);
            decimal st = 0;
            switch (sourceType)
            {
                case DataSizeType.B:
                    st = sv * B;
                    break;
                case DataSizeType.K:
                    st = sv * K;
                    break;
                case DataSizeType.M:
                    st = sv * M;
                    break;
                case DataSizeType.G:
                    st = sv * G;
                    break;
                case DataSizeType.T:
                    st = sv * T;
                    break;
                case DataSizeType.P:
                    st = sv * P;
                    break;
                case DataSizeType.E:
                    st = sv * E;
                    break;
                case DataSizeType.Z:
                    st = sv * Z;
                    break;
                default:
                    st = sv;
                    break;
            }
            switch (targetType)
            {
                case DataSizeType.B:
                    return st / B;
                case DataSizeType.K:
                    return st / K;
                case DataSizeType.M:
                    return st / M;
                case DataSizeType.G:
                    return st / G;
                case DataSizeType.T:
                    return st / T;
                case DataSizeType.P:
                    return st / P;
                case DataSizeType.E:
                    return st / E;
                case DataSizeType.Z:
                    return st / Z;
                default:
                    return st;
            }
        }

        public static string getAutoSizeString(object source, DataSizeType soucetype)
        {
            string s = source.ToString();
            decimal d = Convert.ToDecimal(source);
            DataSizeType type = soucetype;
            while (d > 1000m && type != DataSizeType.Z)
            {
                type = UpgradeSizeType(type);
                d = getSizeString(source, soucetype, type);
            }
            s = d.ToString("f2") + type.ToString();
            return s;
        }
    }
}
