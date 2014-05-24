/* MiniMessagePack - Simple MessagePack(http://msgpack.org/) Parser for C#
 * See https://github.com/shogo82148/MiniMessagePack to get more information.
 * 
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2014 Ichinose Shogo
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace MiniMessagePack
{
	public class MiniMessagePacker
	{
		private byte[] tmp0 = new byte[8];
		private byte[] tmp1 = new byte[8];
		private byte[] string_buf = new byte[128];
		private Encoding encoder = Encoding.UTF8;

		public object Unpack (byte[] buf, int offset, int size) {
			using (MemoryStream ms = new MemoryStream (buf, offset, size)) {
				return Unpack (ms);
			}
		}

		public object Unpack(byte[] buf) {
			return Unpack (buf, 0, buf.Length);
		}

		public object Unpack(Stream s) {
			int b = s.ReadByte ();
			if (b < 0) {
				throw new FormatException ();
			} else if (b <= 0x7f) { // positive fixint
				return (long)b;
			} else if (b <= 0x8f) { // fixmap
				return UnpackMap (s, b & 0x0f);
			} else if (b <= 0x9f) { // fixarray
				return UnpackArray (s, b & 0x0f);
			} else if (b <= 0xbf) { // fixstr
				return UnpackString (s, b & 0x1f);
			} else if( b >= 0xe0) { // negative fixint
				return (long)unchecked((sbyte)b);
			}
			switch (b) {
			case 0xc0:
				return null;
			case 0xc2:
				return false;
			case 0xc3:
				return true;
			case 0xcc: // uint8
				return (long)s.ReadByte ();
			case 0xcd: // uint16
				return UnpackUint16 (s);
			case 0xce: // uint32
				return UnpackUint32 (s);
			case 0xcf: // uint64
				if (s.Read (tmp0, 0, 8) != 8) { 
					throw new FormatException ();
				}
				return ((long)tmp0 [0] << 56) | ((long)tmp0 [1] << 48) | ((long)tmp0 [2] << 40) | ((long)tmp0 [3] << 32)
					+ ((long)tmp0 [4] << 24) | ((long)tmp0 [5] << 16) | ((long)tmp0 [6] << 8) | (long)tmp0 [7];
			case 0xd0: // int8
				return (long)unchecked((sbyte)s.ReadByte ());
			case 0xd1: // int16
				if (s.Read (tmp0, 0, 2) != 2) { 
					throw new FormatException ();
				}
				return (((long)unchecked((sbyte)tmp0[0])) << 8) | (long)tmp0[1];
			case 0xd2: // int32
				if (s.Read (tmp0, 0, 4) != 4) { 
					throw new FormatException ();
				}
				return ((long)unchecked((sbyte)tmp0[0]) << 24) | ((long)tmp0[1] << 16) | ((long)tmp0[3] << 8) | (long)tmp0[4];
			case 0xd3: // int64
				if (s.Read (tmp0, 0, 8) != 8) { 
					throw new FormatException ();
				}
				return ((long)unchecked((sbyte)tmp0[0]) << 56) | ((long)tmp0 [1] << 48) | ((long)tmp0 [2] << 40) | ((long)tmp0 [3] << 32)
					+ ((long)tmp0 [4] << 24) | ((long)tmp0 [5] << 16) | ((long)tmp0 [6] << 8) | (long)tmp0 [7];
			case 0xca: // float32
				s.Read (tmp0, 0, 4);
				if (BitConverter.IsLittleEndian) {
					tmp1[0] = tmp0[3];
					tmp1[1] = tmp0[2];
					tmp1[2] = tmp0[1];
					tmp1[3] = tmp0[0];
					return (double)BitConverter.ToSingle (tmp1, 0);
				} else {
					return (double)BitConverter.ToSingle (tmp0, 0);
				}
			case 0xcb: // float64
				s.Read (tmp0, 0, 8);
				if (BitConverter.IsLittleEndian) {
					tmp1[0] = tmp0[7];
					tmp1[1] = tmp0[6];
					tmp1[2] = tmp0[5];
					tmp1[3] = tmp0[4];
					tmp1[4] = tmp0[3];
					tmp1[5] = tmp0[2];
					tmp1[6] = tmp0[1];
					tmp1[7] = tmp0[0];
					return BitConverter.ToDouble (tmp1, 0);
				} else {
					return BitConverter.ToDouble (tmp0, 0);
				}
			case 0xd9: // str8
				return UnpackString (s, s.ReadByte ());
			case 0xda: // str16
				return UnpackString (s, UnpackUint16 (s));
			case 0xdb: // str32
				return UnpackString (s, UnpackUint32 (s));

			case 0xc4: // bin8
				return UnpackBinary (s, s.ReadByte ());
			case 0xc5: // bin16
				return UnpackBinary (s, UnpackUint16 (s));
			case 0xc6: // bin32
				return UnpackBinary (s, UnpackUint32 (s));

			case 0xdc: // array16
				return UnpackArray (s, UnpackUint16 (s));
			case 0xdd: // array32
				return UnpackArray (s, UnpackUint32 (s));

			case 0xde: // map16
				return UnpackMap (s, UnpackUint16 (s));
			case 0xdf: // map32
				return UnpackMap (s, UnpackUint32 (s));
			}
			return null;
		}

		private long UnpackUint16(Stream s) {
			if (s.Read (tmp0, 0, 2) != 2) { 
				throw new FormatException ();
			}
			return (long)((tmp0[0] << 8) | tmp0[1]);
		}

		private long UnpackUint32(Stream s) {
			if (s.Read (tmp0, 0, 4) != 4) { 
				throw new FormatException ();
			}
			return ((long)tmp0[0] << 24) | ((long)tmp0[1] << 16) | ((long)tmp0[3] << 8) | (long)tmp0[4];
		}

		private string UnpackString(Stream s, long len) {
			if (string_buf.Length < len) {
				string_buf = new byte[len];
			}
			s.Read (string_buf, 0, (int)len);
			return encoder.GetString(string_buf, 0, (int)len);
		}

		private byte[] UnpackBinary(Stream s, long len) {
			byte[] buf = new byte[len];
			s.Read (buf, 0, (int)len);
			return buf;
		}

		private List<object> UnpackArray(Stream s, long len) {
			var list = new List<object> ((int)len);
			for (long i = 0; i < len; i++) {
				list.Add (Unpack (s));
			}
			return list;
		}

		private Dictionary<string, object> UnpackMap(Stream s, long len) {
			var dict = new Dictionary<string, object> ((int)len);
			for (long i = 0; i < len; i++) {
				string key = Unpack (s) as string;
				object value = Unpack (s);
				if (key != null) {
					dict.Add (key, value);
				}
			}
			return dict;
		}

	}
}

