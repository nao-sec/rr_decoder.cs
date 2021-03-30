using System;
using System.IO;
using System.Linq;

namespace rr_decoder
{
    class Decoder
    {
        public static byte[][] signatures = new byte[][]
        {
            new byte[] {0xb0, 0x74, 0x77, 0x46},
            new byte[] {0xb2, 0x5a, 0x6f, 0x00},
            new byte[] {0xb2, 0xa6, 0x6d, 0xff},
            new byte[] {0xf2, 0xa3, 0x20, 0x72},
            new byte[] {0xb2, 0xa4, 0x6e, 0xff},
            new byte[] {0xa9, 0xa4, 0x6e, 0xfe},
            new byte[] {0x94, 0x5f, 0xda, 0xd8},
            new byte[] {0x95, 0xa2, 0x74, 0x8e}
        };

        public static byte[] decode_b0747746(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            uint xor_key = 1219836524;

            for (var i = 0; i < enc_data.Length; i++)
            {
                for (var _ = 0; _ < 7; _++)
                {
                    bool x0 = (xor_key & 0x20000000) == 0x20000000;
                    bool x1 = (xor_key & 8) == 8;
                    uint x2 = xor_key & 1;
                    uint x3 = 1 + (Convert.ToUInt32(x0) ^ Convert.ToUInt32(x1) ^ x2);
                    xor_key += xor_key + x3;
                }

                dec_data[i] = Convert.ToByte(enc_data[i] ^ (xor_key % 256));
            }

            return dec_data;
        }

        public static byte[] decode_b25a6f00(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            for (var i = 0; i < enc_data.Length; i += 2)
            {
                dec_data[i] = Convert.ToByte(enc_data[i] ^ 0xff);
            }

            return dec_data;
        }

        public static byte[] decode_b2a66dff(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            for (var i = 0; i < enc_data.Length; i++)
            {
                dec_data[i] = Convert.ToByte(enc_data[i] ^ 0xfc);
            }

            dec_data[0] = 0x4d;
            dec_data[1] = 0x5a;

            return dec_data;
        }

        public static byte[] decode_f2a32072(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            uint xor_key = 2079624803;

            for (var i = 0; i < enc_data.Length; i++)
            {
                for (var _ = 0; _ < 7; _++)
                {
                    bool x0 = (xor_key & 0x40000000) == 0x40000000;
                    bool x1 = (xor_key & 8) == 8;
                    uint x2 = xor_key & 1;
                    uint x3 = Convert.ToUInt32(x0) ^ Convert.ToUInt32(x1) ^ x2;
                    xor_key += xor_key + x3;
                }

                dec_data[i] = Convert.ToByte(enc_data[i] ^ (xor_key % 256));
            }

            return dec_data;
        }

        public static byte[] decode_b2a46eff(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            for (var i = 0; i < enc_data.Length; i++)
            {
                dec_data[i] = Convert.ToByte(enc_data[i] ^ 0xff);
            }

            dec_data[1] = 0x5a;
            dec_data[2] = 0x90;

            return dec_data;
        }

        public static byte[] decode_a9a46efe(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            for (var i = 0; i < enc_data.Length; i++)
            {
                dec_data[i] = Convert.ToByte(((enc_data[i] ^ 0x7b) + 0x7b) % 256);
            }

            return dec_data;
        }

        public static byte[] decode_945fdad8(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            uint xor_key = 1387678300;

            for (var i = 0; i < enc_data.Length; i++)
            {
                for (var _ = 0; _ < 7; _++)
                {
                    bool x0 = (xor_key & 0x20000000) == 0x20000000;
                    bool x1 = (xor_key & 8) == 8;
                    uint x2 = xor_key & 1;
                    uint x3 = 1 + (Convert.ToUInt32(x0) ^ Convert.ToUInt32(x1) ^ x2);
                    xor_key += xor_key + x3;
                }

                dec_data[i] = Convert.ToByte(enc_data[i] ^ (xor_key % 256));
            }

            return dec_data;
        }

        public static byte[] decode_95a2748e(byte[] enc_data)
        {
            byte[] dec_data = new byte[enc_data.Length];

            uint xor_key = 1404390492;

            for (var i = 0; i < enc_data.Length; i++)
            {
                for (var _ = 0; _ < 7; _++)
                {
                    bool x0 = (xor_key & 0x20000000) == 0x20000000;
                    bool x1 = (xor_key & 8) == 8;
                    uint x2 = xor_key & 1;
                    uint x3 = 1 + (Convert.ToUInt32(x0) ^ Convert.ToUInt32(x1) ^ x2);
                    xor_key += xor_key + x3;
                }

                dec_data[i] = Convert.ToByte(enc_data[i] ^ (xor_key % 256));
            }

            return dec_data;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("[!] Usage: rr_decoder.exe [Input] [Output]");
                return;
            }

            var input_filename = args[0];
            var output_filename = args[1];

            byte[] enc_data;
            byte[] dec_data;

            using (var fs = new FileStream(input_filename, FileMode.Open, FileAccess.Read))
            {
                enc_data = new byte[fs.Length];
                fs.Read(enc_data, 0, enc_data.Length);
            }

            byte[] header = enc_data.Take(4).ToArray();

            if (header.SequenceEqual(Decoder.signatures[0]))
            {
                dec_data = Decoder.decode_b0747746(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[1]))
            {
                dec_data = Decoder.decode_b25a6f00(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[2]))
            {
                dec_data = Decoder.decode_b2a66dff(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[3]))
            {
                dec_data = Decoder.decode_f2a32072(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[4]))
            {
                dec_data = Decoder.decode_b2a46eff(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[5]))
            {
                dec_data = Decoder.decode_a9a46efe(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[6]))
            {
                dec_data = Decoder.decode_945fdad8(enc_data);
            }
            else if (header.SequenceEqual(Decoder.signatures[7]))
            {
                dec_data = Decoder.decode_95a2748e(enc_data);
            }
            else
            {
                Console.WriteLine("[!] Error: Unknown Format");
                return;
            }

            using (var fs = new FileStream(output_filename, FileMode.Create, FileAccess.Write))
            using (var sw = new BinaryWriter(fs))
            {
                sw.Write(dec_data);
            }
        }
    }
}
