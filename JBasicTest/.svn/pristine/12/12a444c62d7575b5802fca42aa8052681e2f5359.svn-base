package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

public class MultiExtendImplementTest {
	private enum InvokeVersion {
		INVOKE_PARENT, INVOKE_CHILD, INVOKE_UNDEFINE
	}

	private enum TestVersion {
		TEST_BASEREF, TEST_INTERFACE
	}

	static class Parameter {
		public InvokeVersion m_invokeVersion;
	}

	static interface IFunction {
		void noReturnFunction(Parameter parm);
	}

	static class Parent implements IFunction {
		public void noReturnFunction(Parameter parm) {
			parm.m_invokeVersion = InvokeVersion.INVOKE_PARENT;
		}
	}

	static class ChildNoImplement extends Parent {
		public void noReturnFunction(Parameter parm) {
			parm.m_invokeVersion = InvokeVersion.INVOKE_CHILD;
		}
	}

	static class ChildDirectImplement extends Parent implements IFunction {
		public void noReturnFunction(Parameter parm) {
			parm.m_invokeVersion = InvokeVersion.INVOKE_CHILD;
		}
	}

	private TestVersion m_testVersion;

	private void testNoReturn(InvokeVersion expectedFlag, Parent baseref) {
		Parameter parm = new Parameter();
		baseref.noReturnFunction(parm);
		assertEquals(expectedFlag, parm.m_invokeVersion);
		m_testVersion = TestVersion.TEST_BASEREF;
	}

	private void testNoReturn(InvokeVersion expectedFlag, IFunction funcRef) {
		Parameter parm = new Parameter();
		funcRef.noReturnFunction(parm);
		assertEquals(expectedFlag, parm.m_invokeVersion);
		m_testVersion = TestVersion.TEST_INTERFACE;
	}

	@Test
	public void testNoReturnByBase() {
		testNoReturn(InvokeVersion.INVOKE_PARENT, (Parent) new Parent());
		testNoReturn(InvokeVersion.INVOKE_CHILD,
				(Parent) new ChildNoImplement());
		testNoReturn(InvokeVersion.INVOKE_CHILD,
				(Parent) new ChildDirectImplement());
		assertEquals(TestVersion.TEST_BASEREF, m_testVersion);
	}

	@Test
	public void testNoReturnByInterface() {
		testNoReturn(InvokeVersion.INVOKE_PARENT, (IFunction) new Parent());
		testNoReturn(InvokeVersion.INVOKE_CHILD,
				(IFunction) new ChildNoImplement());
		testNoReturn(InvokeVersion.INVOKE_CHILD,
				(IFunction) new ChildDirectImplement());
		assertEquals(TestVersion.TEST_INTERFACE, m_testVersion);
	}
}
