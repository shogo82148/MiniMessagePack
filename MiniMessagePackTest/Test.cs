using NUnit.Framework;
using System;
using MiniMessagePack;
using System.Collections.Generic;
using System.Text;

namespace MiniMessagePackTest
{
	[TestFixture ()]
	public class Test
	{
		[Test()]
		public void PackNull()
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (null);
			Assert.AreEqual (1, actual.Length, "length");
			Assert.AreEqual (0xc0, actual[0], "value");
		}

		[Test()]
		[TestCase(false, 0xc2)]
		[TestCase(true, 0xc3)]
		public void PackBoolean(bool value, byte expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)value);
			Assert.AreEqual (1, actual.Length, value + ": length");
			Assert.AreEqual (expected, actual[0], value + ": value");
		}

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
		[TestCase(0,       new byte[] {0x00})]
		[TestCase(127,     new byte[] {0x7f})]
		[TestCase(128,     new byte[] {0xcc, 0x80})]
		[TestCase(255,     new byte[] {0xcc, 0xff})]
		[TestCase(256,     new byte[] {0xcd, 0x01, 0x00})]
		[TestCase(0x7fff,  new byte[] {0xcd, 0x7f, 0xff})]
		[TestCase(-1,      new byte[] {0xff})]
		[TestCase(-32,     new byte[] {0xe0})]
		[TestCase(-33,     new byte[] {0xd0, 0xdf})]
		[TestCase(-128,    new byte[] {0xd0, 0x80})]
		[TestCase(-129,    new byte[] {0xd1, 0xff, 0x7f})]
		[TestCase(-0x8000, new byte[] {0xd1, 0x80, 0x00})]
		public void PackShortValue(int value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)(short)value);
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

		[Test()]
		[TestCase(0,     new byte[] {0x00})]
		[TestCase(127,   new byte[] {0x7f})]
		[TestCase(128,   new byte[] {0xcc, 0x80})]
		[TestCase(255,   new byte[] {0xcc, 0xff})]
		[TestCase(256,   new byte[] {0xcd, 0x01, 0x00})]
		[TestCase(65535, new byte[] {0xcd, 0xff, 0xff})]
		[TestCase(65536, new byte[] {0xce, 0x00, 0x01, 0x00, 0x00})]
		[TestCase(0x7fffffff, new byte[] {0xce, 0x7f, 0xff, 0xff, 0xff})]
		[TestCase(-1,      new byte[] {0xff})]
		[TestCase(-32,     new byte[] {0xe0})]
		[TestCase(-33,     new byte[] {0xd0, 0xdf})]
		[TestCase(-128,    new byte[] {0xd0, 0x80})]
		[TestCase(-129,    new byte[] {0xd1, 0xff, 0x7f})]
		[TestCase(-0x8000, new byte[] {0xd1, 0x80, 0x00})]
		[TestCase(-0x8001, new byte[] {0xd2, 0xff, 0xff, 0x7f, 0xff})]
		public void PackIntValue(int value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)value);
			Assert.AreEqual (expected.Length, actual.Length, value + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], value + ": [" + i + "]");
			}
		}

		[Test()]
		[TestCase(0,     new byte[] {0x00})]
		[TestCase(127,   new byte[] {0x7f})]
		[TestCase(128,   new byte[] {0xcc, 0x80})]
		[TestCase(255,   new byte[] {0xcc, 0xff})]
		[TestCase(256,   new byte[] {0xcd, 0x01, 0x00})]
		[TestCase(65535, new byte[] {0xcd, 0xff, 0xff})]
		[TestCase(65536, new byte[] {0xce, 0x00, 0x01, 0x00, 0x00})]
		[TestCase(0xffffffff, new byte[] {0xce, 0xff, 0xff, 0xff, 0xff})]
		public void PackUintValue(long value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)(uint)value);
			Assert.AreEqual (expected.Length, actual.Length, "length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], "byte: " + i);
			}
		}

		[Test()]
		[TestCase(0,       new byte[] {0x00})]
		[TestCase(127,     new byte[] {0x7f})]
		[TestCase(128,     new byte[] {0xcc, 0x80})]
		[TestCase(255,     new byte[] {0xcc, 0xff})]
		[TestCase(256,     new byte[] {0xcd, 0x01, 0x00})]
		[TestCase(0xffff,  new byte[] {0xcd, 0xff, 0xff})]
		[TestCase(0x10000, new byte[] {0xce, 0x00, 0x01, 0x00, 0x00})]
		[TestCase(0xffffffff, new byte[] {0xce, 0xff, 0xff, 0xff, 0xff})]
		[TestCase(0x100000000, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00})]
		[TestCase(-1,      new byte[] {0xff})]
		[TestCase(-32,     new byte[] {0xe0})]
		[TestCase(-33,     new byte[] {0xd0, 0xdf})]
		[TestCase(-128,    new byte[] {0xd0, 0x80})]
		[TestCase(-129,    new byte[] {0xd1, 0xff, 0x7f})]
		[TestCase(-0x8000, new byte[] {0xd1, 0x80, 0x00})]
		[TestCase(-0x8001, new byte[] {0xd2, 0xff, 0xff, 0x7f, 0xff})]
		[TestCase(-0x80000000, new byte[] {0xd2, 0x80, 0x00, 0x00, 0x00})]
		[TestCase(-0x80000001, new byte[] {0xd3, 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff})]
		public void PackLongValue(long value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)value);
			Assert.AreEqual (expected.Length, actual.Length, value + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], value + ": [" + i + "]");
			}
		}

		[Test()]
		[TestCase((ulong)0,     new byte[] {0x00})]
		[TestCase((ulong)127,   new byte[] {0x7f})]
		[TestCase((ulong)128,   new byte[] {0xcc, 0x80})]
		[TestCase((ulong)255,   new byte[] {0xcc, 0xff})]
		[TestCase((ulong)256,   new byte[] {0xcd, 0x01, 0x00})]
		[TestCase((ulong)65535, new byte[] {0xcd, 0xff, 0xff})]
		[TestCase((ulong)65536, new byte[] {0xce, 0x00, 0x01, 0x00, 0x00})]
		[TestCase((ulong)0xffffffff, new byte[] {0xce, 0xff, 0xff, 0xff, 0xff})]
		[TestCase((ulong)0x100000000, new byte[] {0xcf, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00})]
		public void PackUlongValue(ulong value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ((object)value);
			Assert.AreEqual (expected.Length, actual.Length, value + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], value + ": [" + i + "]");
			}
		}

		[Test()]
		[TestCase( 0.0f, new byte[] { 0xca, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 1.0f, new byte[] { 0xca, 0x3f, 0x80, 0x00, 0x00 })]
		[TestCase(-2.0f, new byte[] { 0xca, 0xc0, 0x00, 0x00, 0x00 })]
		[TestCase( 0x800000, new byte[] { 0xca, 0x4b, 0x00, 0x00, 0x00 })]
		[TestCase( 0x810000, new byte[] { 0xca, 0x4b, 0x01, 0x00, 0x00 })]
		[TestCase( 0x800100, new byte[] { 0xca, 0x4b, 0x00, 0x01, 0x00 })]
		[TestCase( 0x800001, new byte[] { 0xca, 0x4b, 0x00, 0x00, 0x01 })]
		public void PackFloatValue(float value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (value);
			Assert.AreEqual (expected.Length, actual.Length, value + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], value + ": [" + i + "]");
			}
		}

		[TestCase( 0.0, new byte[] { 0xcb, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 1.0, new byte[] { 0xcb, 0x3f, 0xf0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 2.0, new byte[] { 0xcb, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase(-2.0, new byte[] { 0xcb, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 0x10000000000000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 0x10010000000000, new byte[] { 0xcb, 0x43, 0x30, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 0x10000100000000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 })]
		[TestCase( 0x10000001000000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 })]
		[TestCase( 0x10000000010000, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 })]
		[TestCase( 0x10000000000100, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 })]
		[TestCase( 0x10000000000001, new byte[] { 0xcb, 0x43, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 })]
		public void PackDoubleValue(double value, byte[] expected)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (value);
			Assert.AreEqual (expected.Length, actual.Length, value + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], value + ": [" + i + "]");
			}
		}

		[Test()]
		[TestCase("",    new byte[] { 0xa0 })]
		[TestCase("a",   new byte[] { 0xa1, 0x61 })]
		[TestCase("bc",  new byte[] { 0xa2, 0x62, 0x63 })]
		[TestCase("123456789abcdef", new byte[] { 0xaf, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66 })]
		[TestCase("0123456789abcdef", new byte[] { 0xb0, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66 })]
		[TestCase("123456789abcdef0123456789abcdef", new byte[] {
			0xbf,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66,
			0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66,
		})]
		[TestCase("0123456789abcdef0123456789abcdef", new byte[] {
			0xd9, 0x20,
			0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66,
			0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66,
		})]
		[TestCase("\u3042", new byte[] {0xa3, 0xe3, 0x81, 0x82})]
		public void PackString(string value, byte[] expected) {
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (value);
			Assert.AreEqual (expected.Length, actual.Length, value + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], value + ": [" + i + "]");
			}
		}

		[Test()]
		public void PackString255() {
			var str = new StringBuilder ();
			for (int i = 0; i < 255; i++) {
				str.Append ("a");
			}

			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (str.ToString());
			Assert.AreEqual (str.Length + 2, actual.Length);
			Assert.AreEqual (0xd9, actual [0]);
			Assert.AreEqual (0xff, actual [1]);
			for (int i = 0; i < str.Length; i++) {
				Assert.AreEqual (0x61, actual [i + 2]);
			}
		}

		[Test()]
		public void PackString256() {
			var str = new StringBuilder ();
			for (int i = 0; i < 256; i++) {
				str.Append ("a");
			}

			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (str.ToString());
			Assert.AreEqual (str.Length + 1 + 2, actual.Length);
			Assert.AreEqual (0xda, actual [0]);
			Assert.AreEqual (0x01, actual [1]);
			Assert.AreEqual (0x00, actual [2]);
			for (int i = 0; i < str.Length; i++) {
				Assert.AreEqual (0x61, actual [i + 1 + 2]);
			}
		}

		[Test()]
		public void PackString65535() {
			var str = new StringBuilder ();
			for (int i = 0; i < 65535; i++) {
				str.Append ("a");
			}

			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (str.ToString());
			Assert.AreEqual (str.Length + 1 + 2, actual.Length);
			Assert.AreEqual (0xda, actual [0]);
			Assert.AreEqual (0xff, actual [1]);
			Assert.AreEqual (0xff, actual [2]);
			for (int i = 0; i < str.Length; i++) {
				Assert.AreEqual (0x61, actual [i + 1 + 2]);
			}
		}

		[Test()]
		public void PackString65536() {
			var str = new StringBuilder ();
			for (int i = 0; i < 65536; i++) {
				str.Append ("a");
			}

			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (str.ToString());
			Assert.AreEqual (str.Length + 1 + 4, actual.Length);
			Assert.AreEqual (0xdb, actual [0]);
			Assert.AreEqual (0x00, actual [1]);
			Assert.AreEqual (0x01, actual [2]);
			Assert.AreEqual (0x00, actual [3]);
			Assert.AreEqual (0x00, actual [4]);
			for (int i = 0; i < str.Length; i++) {
				Assert.AreEqual (0x61, actual [i + 1 + 4]);
			}
		}

		[Test()]
		[TestCase( new object[]{}, new byte[] { 0x90 }, "empty array" )]
		[TestCase( new object[]{ 1 }, new byte[] { 0x91, 0x01 }, "length 1" )]
		[TestCase( new object[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 
			new byte[] { 0x9f, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f }, "length 15" )]
		[TestCase( new object[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, 
			new byte[] { 0xdc, 0x00, 0x10, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, "length 16" )]
		public void PackArray(object[] array, byte[] expected, string message)
		{
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (array);
			Assert.AreEqual (expected.Length, actual.Length, message + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], message + ": [" + i + "]");
			}
		}

		[Test()]
		public void PackLongArray()
		{
			var array = new int[65535];
			for (int i = 0; i < array.Length; i++) {
				array [i] = 0x00;
			}

			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (array);
			Assert.AreEqual (array.Length + 1 + 2, actual.Length);
			Assert.AreEqual (0xdc, actual [0]);
			Assert.AreEqual (0xff, actual [1]);
			Assert.AreEqual (0xff, actual [2]);
			for (int i = 0; i < array.Length; i++) {
				Assert.AreEqual (0x00, actual [i + 1 + 2]);
			}
		}

		[Test()]
		public void PackVeryLongArray()
		{
			var array = new int[65536];
			for (int i = 0; i < array.Length; i++) {
				array [i] = 0x00;
			}

			var packer = new MiniMessagePacker ();
			var actual = packer.Pack (array);
			Assert.AreEqual (array.Length + 1 + 4, actual.Length);
			Assert.AreEqual (0xdd, actual [0]);
			Assert.AreEqual (0x00, actual [1]);
			Assert.AreEqual (0x01, actual [2]);
			Assert.AreEqual (0x00, actual [3]);
			Assert.AreEqual (0x00, actual [4]);
			for (int i = 0; i < array.Length; i++) {
				Assert.AreEqual (0x00, actual [i + 1 + 4]);
			}
		}

		[Test()]
		[TestCase( new string[] {}, new object[]{}, new byte[] { 0x80 }, "empty map" )]
		[TestCase( new string[] {"a"}, new object[]{ 1 }, new byte[] { 0x81, 0xa1, 0x61, 0x01 }, "length 1" )]
		[TestCase(
			new string[]{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" },
			new object[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 
			new byte[] { 0x8f, 0xa1, 0x31, 0x01, 0xa1, 0x32, 0x02, 0xa1, 0x33, 0x03, 0xa1, 0x34, 0x04,
				0xa1, 0x35, 0x05, 0xa1, 0x36, 0x06, 0xa1, 0x37, 0x07, 0xa1, 0x38, 0x08,
				0xa1, 0x39, 0x09, 0xa1, 0x61, 0x0a, 0xa1, 0x62, 0x0b, 0xa1, 0x63, 0x0c,
				0xa1, 0x64, 0x0d, 0xa1, 0x65, 0x0e, 0xa1, 0x66, 0x0f }, "length 15" )]
		[TestCase(
			new string[]{ "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" },
			new object[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 
			new byte[] { 0xde, 0x00, 0x10, 0xa1, 0x30, 0x00, 0xa1, 0x31, 0x01, 0xa1, 0x32, 0x02, 0xa1, 0x33, 0x03, 0xa1, 0x34, 0x04,
				0xa1, 0x35, 0x05, 0xa1, 0x36, 0x06, 0xa1, 0x37, 0x07, 0xa1, 0x38, 0x08,
				0xa1, 0x39, 0x09, 0xa1, 0x61, 0x0a, 0xa1, 0x62, 0x0b, 0xa1, 0x63, 0x0c,
				0xa1, 0x64, 0x0d, 0xa1, 0x65, 0x0e, 0xa1, 0x66, 0x0f }, "length 16" )]
		public void PackMap(string[] keys, object[] values, byte[] expected, string message)
		{
			var packer = new MiniMessagePacker ();
			var dict = new SortedDictionary<string, object> ();
			for (int i = 0; i < keys.Length; i++) {
				dict.Add (keys [i], values [i]);
			}
			var actual = packer.Pack (dict);
			Assert.AreEqual (expected.Length, actual.Length, message + ": length");
			for (int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], actual [i], message + ": [" + i + "]");
			}
		}

		private class MyClass
		{
			public override string ToString ()
			{
				return "a";
			}
		}

		[Test()]
		public void PackKnownObject() {
			var packer = new MiniMessagePacker ();
			var actual = packer.Pack ( new MyClass() );
			Assert.AreEqual (2, actual.Length);
			Assert.AreEqual (0xa1, actual[0]);
			Assert.AreEqual (0x61, actual[1]);
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

		[Test()]
		public void UnpackExample()
		{
			// it means {"compact":true,"schema":0} in JSON
			var msgpack = new byte[] {
				0x82, 0xa7, 0x63, 0x6f, 0x6d, 0x70, 0x61, 0x63, 0x74, 0xc3,
				0xa6, 0x73, 0x63, 0x68, 0x65, 0x6d, 0x61, 0x00
			};

			var packer = new MiniMessagePacker ();
			object unpacked_data = packer.Unpack (msgpack);
			/*
			 * unpacked_data = new Dictionary<string, object> {
			 *     { "compact", true },
			 *     { "schema", 0},
			 * };
			*/
			var dict = (Dictionary<string, object>)unpacked_data;
			Assert.IsTrue ((bool)dict ["compact"]);
			Assert.AreEqual (0, (long)dict ["schema"]);
		}

		[Test()]
		public void PackExample()
		{
			var unpacked_data = new SortedDictionary<string, object> {
				{ "compact", true },
				{ "schema", 0},
			};

			var packer = new MiniMessagePacker ();
			byte[] msgpack = packer.Pack (unpacked_data);

			// it means {"compact":true,"schema":0} in JSON
			var expected = new byte[] {
				0x82, 0xa7, 0x63, 0x6f, 0x6d, 0x70, 0x61, 0x63, 0x74, 0xc3,
				0xa6, 0x73, 0x63, 0x68, 0x65, 0x6d, 0x61, 0x00
			};

			Assert.AreEqual (expected.Length, msgpack.Length);
			for(int i = 0; i < expected.Length; i++) {
				Assert.AreEqual (expected [i], msgpack [i]);
			}
		}
	}
}

