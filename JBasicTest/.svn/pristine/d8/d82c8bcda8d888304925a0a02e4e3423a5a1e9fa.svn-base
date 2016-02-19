package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;

import org.junit.Test;

public class DefaultValueTest {
	/**
	 * local variable must be initialized before be used no default value will
	 * be set on non-initialized variable otherwise it cannot pass the
	 * compilation
	 */
	@Test
	public void testLocalVariable() {
		int nVal = 0;
		assertEquals(0, nVal);
	}

	static class TestObject {
		public int m_nValue;
		public boolean m_bValue;
	}

	@Test
	public void testMemberVariable() {
		TestObject obj = new TestObject();
		assertEquals(0, obj.m_nValue);
		assertFalse(obj.m_bValue);
	}
}
