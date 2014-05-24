using NUnit.Framework;
using System;
using MiniMessagePack;
using System.Collections.Generic;

namespace MiniMessagePackTest
{
	[TestFixture ()]
	public class Test
	{
		[Test()]
		[TestCase(0,    new byte[] {0x00})]
		[TestCase(127,  new byte[] {0x7f})]
		[TestCase(-1,   new byte[] {0xff})]
		[TestCase(-32,  new byte[] {0xe0})]
		[TestCase(-33,  new byte[] {0xd0, 0xdf})]
		[TestCase(-128, new byte[] {0xd0, 0x80})]
		public void PackSbyteValue(sbyte value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)value);
			Assert.AreEqual (expected.Length, actual.Length, "length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], "byte: " + i);
			}
		}

		[Test()]
		[TestCase(0, new byte[] {0x00})]
		[TestCase(127, new byte[] {0x7f})]
		[TestCase(128, new byte[] {0xcc, 0x80})]
		[TestCase(255, new byte[] {0xcc, 0xff})]
		public void PackByteValue(byte value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)value);
			Assert.AreEqual (expected.Length, actual.Length, "length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], "byte: " + i);
			}
		}

		[Test()]
		[TestCase(0,     new byte[] {0x00})]
		[TestCase(127,   new byte[] {0x7f})]
		[TestCase(128,   new byte[] {0xcc, 0x80})]
		[TestCase(255,   new byte[] {0xcc, 0xff})]
		[TestCase(256,   new byte[] {0xcd, 0x01, 0x00})]
		[TestCase(65535, new byte[] {0xcd, 0xff, 0xff})]
		public void PackUshortValue(int value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)(ushort)value);
			Assert.AreEqual (expected.Length, actual.Length, "length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], "byte: " + i);
			}
		}

		[Test ()]
		[TestCase(0,   new byte[] {0x00}, "min positive fixed int")]
		[TestCase(127, new byte[] {0x7f}, "max positive fixed int")]
		[TestCase(-32, new byte[] {0xe0}, "min negative fixed int")]
		[TestCase(-1,  new byte[] {0xff}, "max negative fixed int")]

		[TestCase(255, new byte[] {0xcc, 0xff}, "uint8")]
		[TestCase(-1,  new byte[] {0xd0, 0xff}, "int8")]

		[TestCase(0x0100, new byte[] {0xcd, 0x01, 0x00}, "uint16")]
		[TestCase(0x0001, new byte[] {0xcd, 0x00, 0x01}, "uint16")]
		[TestCase(0xffff, new byte[] {0xcd, 0xff, 0xff}, "uint16")]
		[TestCase(0x0100, new byte[] {0xd1, 0x01, 0x00}, "int16")]
		[TestCase(0x0001, new byte[] {0xd1, 0x00, 0x01}, "int16")]
		[TestCase(-1,     new byte[] {0xd1, 0xff, 0xff}, "int16")]

		[TestCase(0x01000000, new byte[] {0xce, 0x01, 0x00, 0x00, 0x00}, "uint32")]
		[TestCase(0x00010000, new byte[] {0xce, 0x00, 0x01, 0x00, 0x00}, "uint32")]
		[TestCase(0x00000100, new byte[] {0xce, 0x00, 0x00, 0x01, 0x00}, "uint32")]
		[TestCase(0x00000001, new byte[] {0xce, 0x00, 0x00, 0x00, 0x01}, "uint32")]
		[TestCase(0xffffffff, new byte[] {0xce, 0xff, 0xff, 0xff, 0xff}, "uint32")]
		[TestCase(0x01000000, new byte[] {0xd2, 0x01, 0x00, 0x00, 0x00}, "int32")]
		[TestCase(0x00010000, new byte[] {0xd2, 0x00, 0x01, 0x00, 0x00}, "int32")]
		[TestCase(0x00000100, new byte[] {0xd2, 0x00, 0x00, 0x01, 0x00}, "int32")]
		[TestCase(0x00000001, new byte[] {0xd2, 0x00, 0x00, 0x00, 0x01}, "int32")]
		[TestCase(-1,         new byte[] {0xd2, 0xff, 0xff, 0xff, 0xff}, "int32")]

		[TestCase(0x0100000000000000, new byte[] {0xcf, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, "uint64")]
		[TestCase(0x0001000000000000, new byte[] {0xcf, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, "uint64")]
		[TestCase(0x0000010000000000, new byte[] {0xcf, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00}, "uint64")]
		[TestCase(0x0000000100000000, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00}, "uint64")]
		[TestCase(0x0000000001000000, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00}, "uint64")]
		[TestCase(0x0000000000010000, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00}, "uint64")]
		[TestCase(0x0000000000000100, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00}, "uint64")]
		[TestCase(0x0000000000000001, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01}, "uint64")]

		// ATTENTION!!
		// msgpack spec say that it means 0xffffffffffffffff, but MiniMessagePack returns -1
		// because MiniMessagePack cannot handle uint64
		[TestCase(-1, new byte[] {0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff}, "uint64")]

		[TestCase(0x0100000000000000, new byte[] {0xd3, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, "int64")]
		[TestCase(0x0001000000000000, new byte[] {0xd3, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, "int64")]
		[TestCase(0x0000010000000000, new byte[] {0xd3, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00}, "int64")]
		[TestCase(0x0000000100000000, new byte[] {0xd3, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00}, "int64")]
		[TestCase(0x0000000001000000, new byte[] {0xd3, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00}, "int64")]
		[TestCase(0x0000000000010000, new byte[] {0xd3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00}, "int64")]
		[TestCase(0x0000000000000100, new byte[] {0xd3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00}, "int64")]
		[TestCase(0x0000000000000001, new byte[] {0xd3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01}, "int64")]
		[TestCase(-1,                 new byte[] {0xd3, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff}, "int64")]
		public void IntegerValue (long expected, byte[] data, string message = "")
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Unpack (data);
			Assert.AreEqual (actual.GetType (), typeof(long));
			Assert.AreEqual (expected, actual, message);
		}

		[Test()]
		[TestCase( 0.0, new byte[] { 0xca, 0x00, 0x00, 0x00, 0x00 }, "float 32")]
		[TestCase( 1.0, new byte[] { 0xca, 0x3f, 0x80, 0x00, 0x00 }, "float 32")]
		[TestCase(-2.0, new byte[] { 0xca, 0xc0, 0x00, 0x00, 0x00 }, "float 32")]
		[TestCase( 0x800000, new byte[] { 0xca, 0x4b, 0x00, 0x00, 0x00 }, "float 32")]
		[TestCase( 0x810000, new byte[] { 0xca, 0x4b, 0x01, 0x00, 0x00 }, "float 32")]
		[TestCase( 0x800100, new byte[] { 0xca, 0x4b, 0x00, 0x01, 0x00 }, "float 32")]
		[TestCase( 0x800001, new byte[] { 0xca, 0x4b, 0x00, 0x00, 0x01 }, "float 32")]
		[TestCase( 0.0, new byte[] { 0xcb, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 1.0, new byte[] { 0xcb, 0x3f, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 2.0, new byte[] { 0xcb, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase(-2.0, new byte[] { 0xcb, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 0x10000000000000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 0x10010000000000, new byte[] { 0xcb, 0x43, 0x30, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 0x10000100000000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 0x10000001000000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 }, "float 64")]
		[TestCase( 0x10000000010000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 }, "float 64")]
		[TestCase( 0x10000000000100, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 }, "float 64")]
		[TestCase( 0x10000000000001, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, "float 64")]
		public void FloatingValue(double expected, byte[] data, string message)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Unpack (data);
			Assert.AreEqual (actual.GetType (), typeof(double));
			Assert.AreEqual (expected, actual, message);
		}

		[Test()]
		[TestCase("",    new byte[] { 0xa0 }, "empty string")]
		[TestCase("a",   new byte[] { 0xa1, 0x61 }, "fixed string")]
		[TestCase("bc",  new byte[] { 0xa2, 0x62, 0x63 }, "fixed string")]
		[TestCase("def", new byte[] { 0xa3, 0x64, 0x65, 0x66 }, "fixed string")]
		[TestCase("",    new byte[] {0xda, 0x00, 0x00}, "string 16")]
		[TestCase("a",   new byte[] {0xda, 0x00, 0x01, 0x61}, "string 16")]
		[TestCase("",    new byte[] {0xdb, 0x00, 0x00, 0x00, 0x00}, "string 16")]
		[TestCase("a",   new byte[] {0xdb, 0x00, 0x00, 0x00, 0x01, 0x61}, "string 16")]
		[TestCase("\u3042", new byte[] {0xa3, 0xe3, 0x81, 0x82}, "japanese")]
		public void StringValues(string expected, byte[] data, string message)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Unpack (data);
			Assert.AreEqual (actual.GetType (), typeof(string));
			Assert.AreEqual (expected, actual, message);
		}

		[Test()]
		[TestCase( new object[] {},       new byte[]{ 0x90 }, "fixed array")]
		[TestCase( new object[] { 1 },    new byte[]{ 0x91, 0x01 }, "fixed array")]
		[TestCase( new object[] { 1, 2 }, new byte[]{ 0x92, 0x01, 0x02 }, "fixed array")]
		[TestCase( new object[] {},       new byte[]{ 0xdc, 0x00, 0x00 }, "array16")]
		[TestCase( new object[] { 1 },    new byte[]{ 0xdc, 0x00, 0x01, 0x01 }, "array16")]
		[TestCase( new object[] { 1, 2 }, new byte[]{ 0xdc, 0x00, 0x02, 0x01, 0x02 }, "array16")]
		[TestCase( new object[] {},       new byte[]{ 0xdd, 0x00, 0x00, 0x00, 0x00 }, "array32")]
		[TestCase( new object[] { 1 },    new byte[]{ 0xdd, 0x00, 0x00, 0x00, 0x01, 0x01 }, "array32")]
		[TestCase( new object[] { 1, 2 }, new byte[]{ 0xdd, 0x00, 0x00, 0x00, 0x02, 0x01, 0x02 }, "array32")]
		public void ArrayValues(object[] expected, byte[] data, string message)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Unpack (data) as List<object>;
			Assert.AreNotEqual (actual, null, message + ": type");
			Assert.AreEqual (expected.Length, actual.Count, message + ": count");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], message + "[" + i + "]");
			}
		}

		[Test()]
		[TestCase( new string[] {}, new object[] {}, new byte[] { 0x80 }, "fixed map")]
		[TestCase( new string[] {"a", "b"}, new object[] {1, 2}, new byte[] { 0x82,                         0xa1, 0x61, 0x01, 0xa1, 0x62, 0x02 }, "fixed map")]
		[TestCase( new string[] {"a", "b"}, new object[] {1, 2}, new byte[] { 0xde,             0x00, 0x02, 0xa1, 0x61, 0x01, 0xa1, 0x62, 0x02 }, "map16")]
		[TestCase( new string[] {"a", "b"}, new object[] {1, 2}, new byte[] { 0xdf, 0x00, 0x00, 0x00, 0x02, 0xa1, 0x61, 0x01, 0xa1, 0x62, 0x02 }, "map32")]
		public void MapValues(string[] keys, object[] values, byte[] data, string message)
		{
			var packer = new MiniMessagePacker ();
			var actual = (Dictionary<string, object>)packer.Unpack (data);
			Assert.AreEqual (keys.Length, actual.Count, message + ": count");
			for(int i = 0; i < keys.Length; i++) {
				Assert.AreEqual (values [i], actual [keys[i]], message + "[" + keys[i] + "]");
			}
		}

		[Test()]
		[TestCase( false, new byte[] { 0xc2 })]
		[TestCase( true,  new byte[] { 0xc3 })]
		public void BooleanValues(bool expected, byte[] data)
		{
			var packer = new MiniMessagePacker ();
			var actual = (bool)packer.Unpack (data);
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void NullValue()
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Unpack (new byte[] { 0xc0 });
			Assert.AreEqual (null, actual);
		}
	}
}

