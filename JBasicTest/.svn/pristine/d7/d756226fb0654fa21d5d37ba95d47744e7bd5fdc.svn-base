/**
 * test the syntax of string in java
 * @author cheka
 * @date 2010-3-12,Friday
 */

package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotSame;
import static org.junit.Assert.assertSame;
import static org.junit.Assert.assertTrue;

import java.io.UnsupportedEncodingException;

import org.junit.Test;

public class StringTest {
	@Test
	public void testCreateOnHeap() {
		// since these two strings are created on heap
		// though has the same content
		// but pointing to different objects
		String name1 = new String("cheka");
		String name2 = new String("cheka");

		assertEquals(5, name1.length());

		assertNotSame(name1, name2);
		// "==" can not be overriden in Java, "==" always check the reference
		// equality
		// it will not compare the content
		assertFalse(name1 == name2);

		assertEquals(name1, name2);
		// object's "equals" has been overriden
		assertTrue(name1.equals(name2));
	}

	@Test
	public void testCreateOnStack() {
		// string created on stack will be cached
		// another string with the same content
		// will be pointed to the previous cached one
		String name1 = "cheka";
		String name2 = "cheka";

		assertSame(name1, name2);
		assertTrue(name1 == name2);
	}

	/**
	 * test the "valueof" method in String class convert a number to string
	 */
	@Test
	public void testStringValueof() {
		int intNumber = 100;
		String strNumber = String.valueOf(intNumber);
		assertEquals(intNumber, Integer.parseInt(strNumber));
		assertTrue(intNumber == Integer.parseInt(strNumber));

		float floatNumber = 3.1415926f;
		strNumber = String.valueOf(floatNumber);
		assertEquals(floatNumber, Float.parseFloat(strNumber), 1e-6);
	}

	/**
	 * test the "toString" method in number class convert a number to string
	 */
	@Test
	public void testNumberTostring() {
		String strNumer = Integer.toString(100);
		assertEquals("100", strNumer);
	}

	@Test
	public void testIndex() {
		String name = "cheka";
		char[] characters = name.toCharArray();

		for (int index = 0; index < name.length(); ++index) {
			assertEquals(characters[index], name.charAt(index));
			assertEquals(index, name.indexOf(characters[index]));
		}
	}

	@Test
	public void testSubString() {
		String name = "cheka";

		assertEquals("eka", name.substring(2));

		// substring range from [startindex,endindex-1]
		assertEquals("hek", name.substring(1, 4));

		assertTrue(name.endsWith("ka"));
		assertFalse(name.endsWith("kz"));
	}

	@Test
	public void testIntern() {
		String name1 = "cheka";
		String name2 = new String("cheka");
		assertNotSame(name1, name2);

		name2 = name1.intern();
		assertSame(name1, name2);
	}

	@Test
	public void testCompareIgnoreCase() {
		String name1 = "cheka";
		String name2 = "CHEKA";

		assertFalse(name1.equals(name2));
		assertTrue(name1.equalsIgnoreCase(name2));
		assertEquals(0, name1.compareToIgnoreCase(name2));
	}

	@Test
	public void testFormat() {
		String name = "cheka";
		int id = 5;

		String formatString = String.format("%s-%d", name, id);
		String appendedString = name + "-" + Integer.toString(id);

		assertEquals(appendedString, formatString);
	}

	@Test
	public void testSplit() {
		String compositeString = "cheka.zhao";

		// !!!!!!!!!!!!!!!!!!!!!!!!
		// !!! if we want to split a string by dot
		// !!! we cannot call split by ".", that's because
		// !!! "String" class split a string by using Regular Expression,
		// !!! In a regular expression, a plain dot matches any character.
		// !!! The correct regular expression to match a dot is "\\."
		// !!!!!!!!!!!!!!!!!!!!!!!!
		String[] parts = compositeString.split("\\.");

		assertEquals(2, parts.length);
		assertEquals("cheka", parts[0]);
		assertEquals("zhao", parts[1]);
	}

	@Test
	public void testGetbytes() throws UnsupportedEncodingException
	{
		final String charsetName = "US-ASCII";
		
		String name = "cheka";
		byte[] buffer = name.getBytes(charsetName);
		// if using ASCII, then the length of the buffer should be equal with the original length of the whole string
		assertEquals(name.length(), buffer.length);
		
		String cpyName = new String(buffer,charsetName);
		assertEquals(name, cpyName);
	}
}
