package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

public class InnerClassTest {
	private static class OutterClass {
		private class InnerClass {
			public void action() {
				m_flag = !m_flag;
			}
		}

		private boolean m_flag = false;
		private InnerClass m_toggler = new InnerClass();

		public boolean getFlag() {
			return m_flag;
		}

		public void toggle() {
			m_toggler.action();
		}
	}

	@Test
	public void testInnerClass() {
		OutterClass outobj = new OutterClass();
		assertFalse(outobj.getFlag());

		outobj.toggle();
		assertTrue(outobj.getFlag());
	}

	@Test
	public void testAnonymousClass() {
		Object obj = new Object() {
			@Override
			public String toString() {
				return "AnonymousClass";
			}
		};
		assertEquals("AnonymousClass", obj.toString());
	}
}
