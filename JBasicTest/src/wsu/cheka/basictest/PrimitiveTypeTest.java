package wsu.cheka.basictest;

import junit.framework.TestCase;

import org.junit.Test;

public class PrimitiveTypeTest extends TestCase {
	/**
	 * show the length of each primary type
	 */
	@Test
	public void testTypeLength() {
		assertEquals(16, Short.SIZE);
		assertEquals(32, Integer.SIZE);
		assertEquals(64, Long.SIZE);

		assertEquals(8, Byte.SIZE);

		assertEquals(32, Float.SIZE);
		assertEquals(64, Double.SIZE);

		// use unicode to represent each character
		assertEquals(16, Character.SIZE);
	}

	@Test
	/**
	 * test the derive relationship among primitive types
	 */
	public void testDeriveRelationship() {
		// Integer.class returns a "Class"
		// Integer.TYPE returns a "Class<Integer>"
		// which are totally not same
		assertTrue(Number.class.isAssignableFrom(Integer.class));
		assertFalse(Number.class.isAssignableFrom(Integer.TYPE));

	}

	/**
	 * test boxing problems boxing result of integer between -128~127 will be
	 * cached so object after boxing will point to same object
	 */
	@Test
	public void testSmallIntBoxing() {
		Integer smallInt1 = 100;
		Integer smallInt2 = 100;

		// check whether they points to the same object
		assertSame(smallInt1, smallInt2);
		assertTrue(smallInt1 == smallInt2);

		// check whether two objects are equal or not
		assertEquals(smallInt1, smallInt2);
		assertTrue(smallInt1.equals(smallInt2));
	}

	@Test
	public void testBigIntBoxing() {
		Integer bigInt1 = 200;
		Integer bigInt2 = 200;

		assertNotSame(bigInt1, bigInt2);
		assertFalse(bigInt1 == bigInt2);

		assertEquals(bigInt1, bigInt2);
		assertTrue(bigInt1.equals(bigInt2));
	}
}
