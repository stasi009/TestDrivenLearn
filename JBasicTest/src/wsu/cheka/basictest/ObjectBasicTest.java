package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

enum InvokeFlag {
	INVOKE_PRIM_INT, INVOKE_INTEGER_OBJ
}

public class ObjectBasicTest {
	/**
	 * for test purpose this inner class doesn't use any embed feature make this
	 * class static inner class, can avoid to keep the creator class alive too
	 * much long
	 * 
	 * @author cheka
	 */
	static class TestObject {
		private static int m_numStaticBlockInvoked = 0;
		static {
			++m_numStaticBlockInvoked;
		}

		private boolean m_flag = true;
		private boolean m_isFieldSet = false;
		private InvokeFlag m_invokeFlag;
		private final int m_finalValue;

		public static int getNumStaticBlockInvoked() {
			return m_numStaticBlockInvoked;
		}

		public TestObject() {
			m_isFieldSet = m_flag;
			// just like readonly, it doesn't need to assign a value to the member field
			// when declaring it. Assigning a value to a final member field can be delayed 
			// to constructor
			m_finalValue = 10;
		}

		public TestObject(int constValue) {
			m_isFieldSet = m_flag;
			m_finalValue = constValue;
		}

		public int getFinalValue() {
			return m_finalValue;
		}

		public boolean isFieldSet() {
			return m_isFieldSet;
		}

		public InvokeFlag getInvokeFlag() {
			return m_invokeFlag;
		}

		public void function(int number) {
			m_invokeFlag = InvokeFlag.INVOKE_PRIM_INT;
		}

		public void function(Integer number) {
			m_invokeFlag = InvokeFlag.INVOKE_INTEGER_OBJ;
		}

		public int sum(int... values) {
			int total = 0;
			for (int val : values) {
				total += val;
			}
			return total;
		}
	}

	/**
	 * test that initialization of member field is always before any constructor
	 */
	@Test
	public void testConstructionOrder() {
		TestObject obj = new TestObject();
		assertTrue(obj.isFieldSet());
	}

	/**
	 * check that the static block will be invoked once and only once
	 */
	@Test
	public void testStaticBlockInvoked() {
		for (int index = 0; index < 10; ++index) {
			TestObject obj = new TestObject();
			assertEquals(1, TestObject.getNumStaticBlockInvoked());
		}
	}

	/**
	 * check the resolution order is: 1. first find the matched method that
	 * doesn't need boxing 2. if failed, find the matched method after boxing
	 * 
	 * always find the most matched one to invoke
	 */
	@Test
	public void testParmBoxingOverload() {
		TestObject obj1 = new TestObject();
		obj1.function(1);
		assertEquals(InvokeFlag.INVOKE_PRIM_INT, obj1.getInvokeFlag());

		TestObject obj2 = new TestObject();
		obj2.function(Integer.valueOf(1));
		assertEquals(InvokeFlag.INVOKE_INTEGER_OBJ, obj2.getInvokeFlag());
	}

	@Test
	public void testNonFixedParameters() {
		TestObject obj = new TestObject();
		assertEquals(3, obj.sum(1, 2));
		assertEquals(5, obj.sum(2, 2, 1));
		assertEquals(10, obj.sum(2, 3, 4, 1));
	}

	/**
	 * test that the protected member variable and member variable with default
	 * access privilege can be accessed with the same package
	 */
	@Test
	public void testAccessPrivilege() {
		AccessTestObject obj = new AccessTestObject();
		obj.m_protectField = 5;
		obj.m_defaultAccessField = 6;
	}

	/**
	 * "final" keyword in Java plays both role of the "const" and "readyonly"
	 * keyword in C#
	 */
	@Test
	public void testReadonly() {
		int number = 10;
		TestObject obj = new TestObject(number);
		assertEquals(number, obj.getFinalValue());
	}
}
