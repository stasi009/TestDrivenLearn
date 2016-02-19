package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotSame;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

public class EqualTest {
	static class OverrideEqualObj {
		private int m_number;

		public OverrideEqualObj(int number) {
			m_number = number;
		}

		/**
		 * add @Override to force the compiler to check this method's signature
		 * match one method in supper class not generate a new signature by
		 * mistake
		 */
		@Override
		public boolean equals(Object right) {
			// it's much more convenient that Java doesn't allow
			// operator overriding, which save us a lot of trouble
			// == can always used to check reference equality
			if (right == this)
				return true;

			if (!(right instanceof OverrideEqualObj))
				return false;

			final OverrideEqualObj overrideObj = (OverrideEqualObj) right;
			return this.m_number == overrideObj.m_number;
		}

		@Override
		public int hashCode() {
			return Integer.valueOf(m_number).hashCode();
		}
	}

	static class OriginalEqualObj {
		private int m_number;

		public OriginalEqualObj(int number) {
			m_number = number;
		}

		public static boolean isEqual(OriginalEqualObj obj1,
				OriginalEqualObj obj2) {
			return obj1.m_number == obj2.m_number;
		}
	}

	@Test
	public void testOriginalEqual() {
		int number = 100;
		Object obj1 = new OriginalEqualObj(number);
		Object obj2 = new OriginalEqualObj(number);

		assertNotSame(obj1, obj2);
		// == can be always used for reference equality check
		assertFalse(obj1 == obj2);
		assertTrue(OriginalEqualObj.isEqual((OriginalEqualObj) obj1,
				(OriginalEqualObj) obj2));
	}

	@Test
	public void testOverrideEqual() {
		int number = 200;

		Object obj1 = new OverrideEqualObj(number);
		Object obj2 = new OverrideEqualObj(number);

		// reference equality checking
		assertNotSame(obj1, obj2);
		assertFalse(obj1 == obj2);

		// use-defined value equality checking
		assertTrue(obj1.equals(obj2));
		assertEquals(obj1, obj2);

		// self equality checking
		assertTrue(obj1.equals(obj1));
	}
}
