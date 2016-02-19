package wsu.cheka.basictest;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

public class ExceptionTest {
	private static class TestObject {
		private boolean m_flag = false;

		int getNumber() {
			try {
				return 100;
			} finally {
				m_flag = true;
			}
		}

		/**
		 * its very strange that even throw an exception that not included in
		 * "throws list" can pass the compilation
		 * 
		 * @throws ArrayIndexOutOfBoundsException
		 */
		void funThrowArrayIndexException()
				throws ArrayIndexOutOfBoundsException {
			throw new ArithmeticException();
		}
	}

	/**
	 * test finally block will be always be invoked before return clause
	 */
	@Test
	public void testReturnFinally() {
		TestObject obj = new TestObject();
		assertFalse(obj.m_flag);

		obj.getNumber();
		assertTrue(obj.m_flag);
	}

	@Test
	public void testCatchOrder() {
		boolean isArrayIndexExpCaught = false;
		boolean isArithmeticExpCaught = false;

		try {
			try {
				throw new ArrayIndexOutOfBoundsException();
			} catch (ArithmeticException e) {
			}
			// this exception will be never thrown
			throw new ArithmeticException();
		} catch (ArithmeticException e) {
			isArithmeticExpCaught = true;
		} catch (ArrayIndexOutOfBoundsException e) {
			isArrayIndexExpCaught = true;
		}

		assertFalse(isArithmeticExpCaught);
		assertTrue(isArrayIndexExpCaught);
	}

	/**
	 * it's strange that the compiler doesn't force us to handle the exception
	 * declared in the "throws list" and even if the method throw an exception
	 * not included in "throws list", the program still pass the compilation
	 */
	@Test
	public void testThrows() {
		boolean isExCaught = false;
		TestObject obj = new TestObject();
		try {
			obj.funThrowArrayIndexException();
		} catch (ArithmeticException ex) {
			isExCaught = true;
		}
		assertTrue(isExCaught);
	}

	private void switchFunction(int number) {
		switch (number) {
		case 1:
			break;

		case 2:
			break;

		default:
			assert false : "not recognized number";
		}
	}

	/**
	 * assert will not run by default to enable assertion checking, we must give
	 * -ea option to jvm Run -> Run... -> Arguments, and in the box labeled VM
	 * arguments enter either -enableassertions or just -ea. Accept the changes
	 * and close the dialog
	 */
	@Test(expected = AssertionError.class)
	public void testAssertException() {
		switchFunction(3);
	}
}
